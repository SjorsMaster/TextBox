using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
[HelpURL("http://sjors.eu/#contact")]

//This script requires certain components, if they don't exist, it should make it's down.
[RequireComponent(typeof(Text))]
[RequireComponent(typeof(AudioSource))]
public class textDisplay : MonoBehaviour
{
    //UI Elements
    private Text UIText;

    //AudioSources
    [SerializeField] AudioClip[] Sounds;
    [SerializeField] AudioClip[] Alphabet;
    private AudioSource audioSource;

    //Numbers
    [SerializeField] float defaultDelay = 0.2f;
    private float currentDelay;
    public List<float> cachedDelays;
    public List<int> cachedStarts;
    private int count = 0;
    private int charcount = 0;

    //Strings
    private string textMessage;
    private string currentText = "";

    //Regex
    private Regex regexObj = new Regex(@"[^0-9.]");


    //Get basic stuff for the script to work.
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        UIText = GetComponent<Text>();
        UIText.text = "";
        currentDelay = defaultDelay;
        rpgText.newTextValue += fetchText;
    }

    //Cleanup, and starting on recieving a new line from the source.
    void fetchText(string text)
    {
        charcount = -1;
        charcount = 0;
        count = 0;
        currentDelay = defaultDelay;
        UIText.text = "";
        if (cachedDelays.Count != 0)
        {
            cachedDelays.Clear();
            cachedStarts.Clear();
        }
        textMessage = text;
        delayChecker();
        StartCoroutine(DisplayText());
    }

    //A check for slower appearing speed, aka delay
    void delayChecker()
    {
        string[] tmpSplit = textMessage.Split(); //Split the words in an temporary array
        for (int i = 0; tmpSplit.Length > i; i++)//Go through them all
        {
            if (tmpSplit[i].Contains("<speed"))//Check if a speed parameter "<speed" is included within the text. If this is the case..
            {
                cachedDelays.Add(float.Parse(regexObj.Replace(tmpSplit[i], "")));//Remove every character except for numbers and dots, then add it to the list
                cachedStarts.Add(textMessage.IndexOf("<speed"));//check for where in the text this appears
                textMessage = textMessage.Replace("<speed=" + cachedDelays[count] + "f>", "");//remove it from the UI message, so there's no trace of it
                count++;//Go for the next one
            }
        }
        count = 0;
    }

    //This bit of code allows for the text you see on the screen
    IEnumerator DisplayText()
    {
        //A for loop for each independent letter
        for (int i = 0; i < textMessage.Length + 1; i++)
        {
            //Escape sequense to quit the enumerator
            if (charcount == -1)
            {
                yield break;
            }
            //This is a section I really don't like, but it works.
            //Check so things don't go bad lol
            if (cachedStarts.Count != 0 && i == cachedStarts[count])
            {
                currentDelay = cachedDelays[count];//Update the delay counts, 
                //which stands essentially equal to the speed of the speech, but technically its a delay because of the wait in the enumerator..
                if (count < cachedStarts.Count - 1 || count == 0 && cachedStarts.Count != 1)//Keep track of the starting points of the speeches, see if we're at one.
                {
                    count++;//If so, get ready for the next one.
                }
            }
            charcount++;//Update the letter count
            try
            {
                Speech();//Execute voices
            }
            catch
            {
                yield break;//Fallback in case it overflows, usually happens because the enumerator doesn't actually stop, then clashes with the other. Not sure why?
            }
            UIText.text = currentText;//Update UI text
            yield return new WaitForSeconds(currentDelay);//Wait for the delay, then do the next letter.
        }
    }

    //This is the part of the code that allows for the voice bits
    void Speech()
    {
        currentText = textMessage.Substring(0, charcount);//This will look for the last character.

        if (currentText.Substring(currentText.Length - 1) != " ")//Check wether the char is a whitespace or not, if it's not
        {
            if (PlayerPrefs.GetInt("Human") == 1)//check wether the human alphabetic letters are enabled, if so...
            {
                string temprament = currentText.Substring(currentText.Length - 1); //get the last letter
                char c = temprament.ToCharArray()[0]; //Convert it to an useable character for the next bit
                int index = char.ToUpper(c) - 64;//Convert it to a number to figure out which letter it is
                audioSource.clip = Alphabet[index - 1]; //Fetch the corrisponding letter with the number
                audioSource.pitch = (UnityEngine.Random.Range(1.85f, 2.15f)); //Pitch the audio for some variation
            }
            else//If not
            {
                audioSource.clip = Sounds[UnityEngine.Random.Range(0, Sounds.Length)];//Use generic boh sound, pick one of two randomly for variation
                audioSource.pitch = (UnityEngine.Random.Range(0.6f, .9f));//More variation
            }
            audioSource.Play();//Play the audio snippit!
        }
    }
}

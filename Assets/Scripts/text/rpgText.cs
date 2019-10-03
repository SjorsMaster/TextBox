using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("http://sjors.eu/#contact")]
public class rpgText : MonoBehaviour
{
    //Get delegates to pass through the text messages
    public delegate void updateText(string text);
    public static event updateText newTextValue;

    //Track on which line of the txt file to be in
    int lineCounter = -1;

    //store the imported text in chunks
    string[] ImportedText;

    //Text file storing
    TextAsset textFile, Memory;

    bool Active, Fallback;

    [Header("Required")]
    [Tooltip("Please, specify the textbar and it's target position on the UI.")]
    [SerializeField] GameObject textbar, target;
    Vector3 savedPosition;

    void Awake()//Initial setup
    {
        if (textbar == null || target == null)
        {
            Debug.LogWarning("Please, specify it's goal target and the textbar itself!");
            GetComponent<rpgText>().enabled = false;
        }

        npc.newTextFile += textSource;
        savedPosition = textbar.transform.position;
        tmpMove.interract += Interraction;
    }

    void Update()//Loop through both statuses
    {
        fileCheck();
        status();
    }

    void status()
    {
        if (Active)//Check if its active
        {
            moveToGoal(textbar, target.transform.position);//ifso move to target
        }
        else
        {
            moveToGoal(textbar, savedPosition);//if not go back to the beginning and
            if (Fallback)//In case something happens, undo EVERYTHING
            {
                textFile = null;
                lineCounter = -1;
                ImportedText = null;
                Fallback = false;
            }
        }
    }

    void fileCheck()
    {
        if (textFile != null)//check if the text file exist
        {
            ImportedText = textFile.ToString().Split("\n"[0]); //if so chop it up in chunks

        }
        else if (Memory != textFile)//keep track wether we're still on the same text file
        {
            Active = false; //Disable the action
            Fallback = true; //Revert everything
        }
    }

    void Interraction()
    {
        if (textFile != null && ImportedText.Length > lineCounter) //double check if it isn't empty, and the lenght of it.
        {
            Fallback = false;//Setting it back to false now that we have new "content" to work with
            if (Active && lineCounter >= 1) //check wether we can do something
            {
                lineCounter++;//Next line(since we start on -1)
                if (lineCounter < ImportedText.Length)//double check wether we're not out of range
                {
                    newTextValue.Invoke(ImportedText[lineCounter]);//Send through the next text
                }
                else
                {
                    Active = false; // disable pretty much any active process
                    Fallback = true; //revert
                }
            }
            else
            {
                if (textFile != null) // if we have something
                {
                    StartCoroutine(waitForAppearance());//Intro fly in
                }
            }
        }
    }

    void textSource(TextAsset sourceFile)
    {
        Memory = textFile;//Store for track keeping purposes
        textFile = sourceFile; //Update source
    }
       
    void moveToGoal(GameObject toBeMoved, Vector3 targetPosition)
    {
        toBeMoved.transform.position = Vector3.Lerp(toBeMoved.transform.position, targetPosition, .035f); //Smoothly move
    }

    IEnumerator waitForAppearance() //fly in on screen and show text
    {
        newTextValue.Invoke("");//Empty old text first
        Active = true;//set the script on active to keep track
        yield return new WaitForSeconds(.5f);//Wait a little before the text appears
        lineCounter++;//Count up the line (we start on -1, which does not exist, but its a failsafe)
        if (lineCounter < ImportedText.Length)//Compare the count to lenght
        {
            newTextValue.Invoke(ImportedText[lineCounter]);//sent through the current line.
        }
        else
        {
            Active = false; //put back the UI element 
            Fallback = true; //revert everything
        }

    }

}

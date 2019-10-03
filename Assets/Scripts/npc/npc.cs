using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("http://sjors.eu/#contact")]
public class npc : MonoBehaviour
{
    [Header("Required:")]
    [Tooltip("Specify the head of this model")]
    [SerializeField] GameObject head;
    [Tooltip("Specify the player")]
    [SerializeField] GameObject player;
    [Tooltip("This is a variable that requests a text file, this is required. Supported formatting: <color>, <i>, <speed=1>, etc.")]
    [SerializeField] TextAsset textFile;
    [Tooltip("In here you should set the default look direction.")]
    [SerializeField] Quaternion defaultLook;

    [Header("Optional:")]
    [Tooltip("Specify the interraction text")]
    [SerializeField] GameObject infoText;
    [Tooltip("Looking range rotation offset.")]
    [SerializeField] float offset;
    [Tooltip("Head turning speed")]
    [SerializeField] float speed = .05f;
    [Tooltip("Pick a language, 0 = BipBop, 1 = Alphabetic")]
    [Range(0, 1)]
    [SerializeField] int lang;

    //Fancy delegates to send through the text file
    public delegate void updateTextFile(TextAsset text);
    public static event updateTextFile newTextFile;

    Quaternion toRotation;
    Vector3 direction;
    bool received;

    private void Awake()
    {
        if (textFile == null || player == null || head == null || defaultLook == null) //Check for the requirements
        {
            Debug.LogWarning("Please, specify a text file, player, head and default look rotation.");
            GetComponent<npc>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        //wonky calcualtions to get the rotation to the player's position.
        direction = player.transform.position - head.transform.position;
        toRotation = Quaternion.LookRotation(transform.up, direction);

        //check if the player is in lookin range, should probably add debug.drawlines or something to make it visual..
        if (Vector3.Distance(head.transform.position, player.transform.position) <= 5 && toRotation.x + offset < .4f && toRotation.x + offset > -.4f)
        {
            if (infoText != null)//check wether the the info text exist, if so, enable it
            {
                infoText.SetActive(true);
            }
            head.transform.rotation = Quaternion.Lerp(head.transform.rotation, toRotation, speed); //look at player
            if (!received)//check if it has already been sent out
            {
                PlayerPrefs.SetInt("Human", lang);//set language
                newTextFile.Invoke(textFile);//send through the text to the manager
                received = true;//make sure you don't do it again
            }
        }
        else
        {
            if (received)//if already sent out, and the player is out of range
            {
                if (infoText != null)//check if theres an info text
                {
                    infoText.SetActive(false);//remove the popup text above npc
                }
                newTextFile.Invoke(null);//remove the current text
                received = false;//make sure the script can sent out again.
            }
            head.transform.rotation = Quaternion.Lerp(head.transform.rotation, defaultLook, speed / 100); //look to the default direction
        }
    }
}

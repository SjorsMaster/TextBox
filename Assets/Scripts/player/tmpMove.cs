using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("http://sjors.eu/#contact")]
[RequireComponent(typeof(playerInput))]
public class tmpMove : MonoBehaviour
{
    playerInput inputManager;

    //event for interraction.
    public delegate void input();
    public static event input interract;

    private void Awake()
    {
        inputManager = GetComponent<playerInput>();//store reference input manager
        if(inputManager == null)//double check wether it actually exists
        {
            Debug.LogWarning("How did you even do this, the playerInput script is required. Please re-attach this script.");
            GetComponent<playerInput>().enabled = false;
        }
    }

    void Update()
    {
        transform.position += new Vector3(inputManager.lDirection.x, 0, inputManager.lDirection.y) * Time.deltaTime * 3; //move around on input

        if (inputManager.interaction)
        {
            interract.Invoke();//notify if the interact button has been pressed.
        }
    }
}

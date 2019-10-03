using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("http://sjors.eu/#contact")]
public class cameraScript : MonoBehaviour
{
    [Header("Required")]
    [Tooltip("Assign the player object")]
    [SerializeField] GameObject Player;

    private void Awake()
    {
        if(Player == null)//if the player is not assigned, disable this component
        {
            Debug.LogWarning("Please, assign the player.");
            GetComponent<cameraScript>().enabled = false;
        }
    }

    void Update()
    {
        transform.LookAt(Player.transform);// keep player in focus
    }
}

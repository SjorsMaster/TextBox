using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("http://sjors.eu/#contact")]

//A silly little script for timecycle
public class timePassing : MonoBehaviour
{
    [SerializeField] float speed = 0.01f; //CycleSpeed
    float timer; //timer to keep track
    void FixedUpdate()
    {
        //Check the time between set two points
        if ((timer * speed / 24) * 360 - 0 < 90 && GetComponent<Light>().intensity < 1.5)
        {
            GetComponent<Light>().intensity += 0.05f * speed;
        }
        else if ((timer * speed / 24) * 360 - 0 > 90 && GetComponent<Light>().intensity > 0)
        {
            GetComponent<Light>().intensity -= 0.05f * speed;
        }

        timer += 0.1f; //pass time

        if((timer * speed / 24) * 360 - 0 == 360) //if time passed, set back to 0
        {
            timer = 0;
        }

        transform.localRotation = Quaternion.Euler(((timer * speed / 24) * 360 - 0), ((timer * (speed / 2) / 24) * 360 - 0), 0); //set the rotation based on time
    }
}

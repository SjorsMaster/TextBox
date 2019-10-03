using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("http://sjors.eu/#contact")]
public class floatText : MonoBehaviour
{   
    Vector3 _startPosition;

    [Header("Required")]
    [Tooltip("Specify it's target to look at")]
    [SerializeField] GameObject cameraTarget;

    private void Awake()
    {
        _startPosition = transform.position; // save position, so the up and down can base on it
        if(cameraTarget == null)//check if the camera is specifed
        {
            Debug.LogWarning("Please, Specify a camera.");
            GetComponent<floatText>().enabled = false;
        }
    }

    void Update()
    {
        transform.LookAt(cameraTarget.transform);//Look at the camera
        transform.position = _startPosition + new Vector3(0, Mathf.Sin(Time.time), 0);//Move up and down
    }
}

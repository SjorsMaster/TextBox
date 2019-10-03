using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("http://sjors.eu/#contact")]
public class playerInput : MonoBehaviour
{
    [Header("Required:")]
    [Tooltip("Please, select which key should correspond to the interaction key")]
    [SerializeField] string interactButton;

    //Publicly accessable inputs for everyone, though, no-one is allowed to set it.
    [HideInInspector] public bool interaction { get; private set; }
    [HideInInspector] public Vector2 lDirection { get; private set; }
    
    void Update()
    {
        interaction = Input.GetKeyDown(interactButton); //toggle key on input
        lDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //convert input to vector2, for easy access.
    }
}

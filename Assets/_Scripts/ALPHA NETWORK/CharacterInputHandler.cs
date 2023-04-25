using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isMouse1ButtonPressed = false;
    bool isMouse2ButtonPressed = false;

    PlayerMovement characterMovementHandler;

    private void Awake()
    {
        characterMovementHandler = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //viewInputVector.x = Input.GetAxis("Mouse X");
        //viewInputVector.y = Input.GetAxis("Mouse Y") - 1; // invert the mouse look

        //characterMovementHandler.SetViewInputVector(viewInputVector);

        if (!characterMovementHandler.Object.HasInputAuthority) 
            return;

        // move input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        //mouse 1 button
        if (Input.GetButtonDown("Fire1"))
            isMouse1ButtonPressed = true;

        //mouse 2 button
        if (Input.GetButtonDown("Fire2"))
            isMouse2ButtonPressed = true;
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        //networkInputData.rotationInput = viewInputVector.x;
        networkInputData.movementInput = moveInputVector;

        // mouse 1 data
        networkInputData.isMouse1ButtonPressed = isMouse1ButtonPressed;

        // mouse 2 data
        networkInputData.isMouse2ButtonPressed = isMouse2ButtonPressed;

        // reset variables now that we have read their states
        isMouse1ButtonPressed = false;

        return networkInputData;
    }
}

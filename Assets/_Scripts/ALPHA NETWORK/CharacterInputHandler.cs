using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;

    CharacterMovementHandler characterMovementHandler;

    private void Awake()
    {
        characterMovementHandler = GetComponent<CharacterMovementHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") - 1; // invert the mouse look

        characterMovementHandler.SetViewInputVector(viewInputVector);

        // move input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        networkInputData.rotationInput = viewInputVector.x;
        networkInputData.movementInput = moveInputVector;

        return networkInputData;
    }
}

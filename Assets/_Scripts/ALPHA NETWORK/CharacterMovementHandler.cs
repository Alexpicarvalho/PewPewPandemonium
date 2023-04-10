using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    NetworkCharacterControllerPrototype networkCharacterControllerPrototype;
    Camera localCamera;

    Vector2 viewInput;
    //float cameraRotationX = 0f;

    private void Awake()
    {
        networkCharacterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
        localCamera = GetComponentInChildren<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //cameraRotationX += viewInput.y * Time.deltaTime;
        //cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);

        //localCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);
    }

    public override void FixedUpdateNetwork()
    {
       //get input from the network
       if (GetInput(out NetworkInputData networkInputData))
        {
            //rotate
            networkCharacterControllerPrototype.Rotate(networkInputData.rotationInput);

            //move
            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterControllerPrototype.Move(moveDirection);

            // check if the player fell of the world
            CheckFallRespawn();
        }
    }

    void CheckFallRespawn()
    {
        if (transform.position.y < -12)
        {
            transform.position = Utils.GetRandomSpawnPoint();
        }
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterInputHandler : MonoBehaviour
{

    [SerializeField] LayerMask _mouseRaycastLayer;

    Vector2 moveInputVector = Vector2.zero;
    Vector3 mousePosition = Vector3.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isMouse1ButtonPressed = false;
    bool isMouse2ButtonPressed = false;
    bool isDodgePressed = false;
    bool isGrenadePressed = false;
    bool isUtilityPressed = false;
    bool isMeleePressed = false;
    bool isWeaponSwapPressed = false;
    bool isInteractPressed = false;
    bool isReloadPressed = false;

    PlayerInputHandler _playerInputHandler;
    PlayerMovement characterMovementHandler;

    private void Awake()
    {
        characterMovementHandler = GetComponent<PlayerMovement>();
        _playerInputHandler = GetComponent<PlayerInputHandler>();

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

        // rotation input

        mousePosition = MousePosition();

        //mouse 1 button
        if (Input.GetButton("Fire1"))
            isMouse1ButtonPressed = true;

        //mouse 2 button
        if (Input.GetButtonDown("Fire2"))
            isMouse2ButtonPressed = true;

        //Dodge Key
        if (Input.GetKeyDown(characterMovementHandler._dodgeKey))
            isDodgePressed = true;

        //Combat Inputs

        if (Input.GetKeyDown(_playerInputHandler._grenadeKey))
            isGrenadePressed = true;

        if (Input.GetKeyDown(_playerInputHandler._utilityKey))
            isUtilityPressed = true;

        if (Input.GetKeyDown(_playerInputHandler._meleeKey))
            isMeleePressed = true;

        if (Input.GetKeyDown(_playerInputHandler._interactKey))
            isInteractPressed = true;

        if (Input.GetKeyDown(_playerInputHandler._weaponSwapKey))
            isWeaponSwapPressed = true;
        
        if (Input.GetKeyDown(_playerInputHandler._reloadKey))
            isReloadPressed = true;

    }

    public Vector3 MousePosition()
    {
        Camera camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Plane plane = new Plane(Vector3.up, transform.position);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _mouseRaycastLayer))
        {
            return new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        else return Vector3.zero;
    }


    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        //networkInputData.rotationInput = viewInputVector.x;
        networkInputData.movementInput = moveInputVector;

        // rotation data
        networkInputData.mousePosition = mousePosition;

        // mouse 1 data
        networkInputData.isMouse1ButtonPressed = isMouse1ButtonPressed;

        // mouse 2 data
        networkInputData.isMouse2ButtonPressed = isMouse2ButtonPressed;

        // dodge key data
        networkInputData.isDodgePressed = isDodgePressed;

        networkInputData.isGrenadePressed = isGrenadePressed;
        networkInputData.isUtilityPressed = isUtilityPressed;
        networkInputData.isMeleePressed = isMeleePressed;
        networkInputData.isWeaponSwapPressed = isWeaponSwapPressed;
        networkInputData.isInteractPressed = isInteractPressed;
        networkInputData.isReloadPressed = isReloadPressed;



        // reset variables now that we have read their states
        isMouse1ButtonPressed = false;
        isMouse2ButtonPressed = false;
        isDodgePressed = false;
        isGrenadePressed = false;
        isUtilityPressed = false;
        isMeleePressed = false;
        isWeaponSwapPressed = false;
        isInteractPressed = false;
        isReloadPressed = false;

        return networkInputData;
    }
}

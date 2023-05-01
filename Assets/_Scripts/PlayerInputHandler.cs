using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Changeable Key Mapping")]
    public KeyCode _dodgeKey; 
    public KeyCode _grenadeKey; 
    public KeyCode _utilityKey; 
    public KeyCode _weaponSwapKey; 
    public KeyCode _interactKey; 
    public KeyCode _meleeKey = KeyCode.V; 
    private void Awake()
    {
        LoadInputMap();
    }

    private void LoadInputMap()
    {
        //Reads File containing Control Settings
    }

    private void SaveInputMap()
    {
        //If there are any changes, rewrites the controil settings file
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerInputHandler : NetworkBehaviour
{
    [Header("Changeable Key Mapping")]
    public KeyCode _dodgeKey; 
    public KeyCode _grenadeKey; 
    public KeyCode _utilityKey; 
    public KeyCode _weaponSwapKey; 
    public KeyCode _interactKey; 
    public KeyCode _meleeKey = KeyCode.V; 
    public KeyCode _reloadKey = KeyCode.R;

    public bool _hasInputAuthority;
    public bool _hasStateAuthority;
    private void Awake()
    {
        LoadInputMap();
    }

    public override void Spawned()
    {
        base.Spawned();
        _hasInputAuthority = HasInputAuthority;
        _hasStateAuthority = HasStateAuthority;
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

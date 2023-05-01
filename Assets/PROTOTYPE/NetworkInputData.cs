using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    //public const byte MOUSEBUTTON1 = 0x01;
    //public const byte MOUSEBUTTON2 = 0x02;

    //public byte buttons;
    //public Vector3 direction;

    public Vector2 movementInput;
    public Vector3 mousePosition;
    public NetworkBool isMouse1ButtonPressed;
    public NetworkBool isMouse2ButtonPressed;
    public NetworkBool isDodgePressed;
    public NetworkBool isGrenadePressed;
    public NetworkBool isUtilityPressed;
    public NetworkBool isMeleePressed;
}

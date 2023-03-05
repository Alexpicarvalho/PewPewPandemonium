using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    //This is a temporary state machine that is to be replaced with a State - Transition method







    public enum MovementStates
    {
        Idle, Moving, Rolling, Locked
    }

    public enum CombatStates
    {
        Idle, Shooting, Casting, Locked
    }

    public enum GeneralStates
    {
        Alive, Dead
    }
}

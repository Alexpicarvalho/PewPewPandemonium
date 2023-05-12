using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerFSM : NetworkBehaviour
{
    [Networked] public MovementState movementState { get; set; }
    [Networked] public CombatState combatState {get;set;}
    [Networked] public GeneralState generalState {get;set;}


    public override void Spawned()
    {
        movementState = MovementState.Idle;
        combatState = CombatState.Idle;
        generalState = GeneralState.Alive;

    }

    public void TransitionState(MovementState newState)
    {
        //if (movementState == MovementState.Locked) return;
        movementState = newState;

    }
    public void TransitionState(CombatState newState)
    {
        //if (combatState == CombatState.Locked) return;
        combatState = newState;
    }
    public void TransitionState(GeneralState newState)
    {
        generalState = newState;
        ApplyGeneralStateRestrictions();
    }

    private void ApplyMovementStateRestrictions()
    {
        switch (movementState)
        {
            case MovementState.Idle:
                break;
            case MovementState.Moving:
                break;
            case MovementState.Rolling:
                break;
            case MovementState.Locked:
                break;
            default:
                break;
        }
    }

    private void ApplyGeneralStateRestrictions()
    {
        switch (generalState)
        {
            case GeneralState.Alive:
                movementState = MovementState.Idle;
                combatState = CombatState.Idle;
                break;
            case GeneralState.Dead:
                movementState = MovementState.Locked;
                combatState = CombatState.Locked;
                break;
            case GeneralState.Stunned:
                movementState = MovementState.Locked;
                combatState = CombatState.Locked;
                break;
        }
    }


    public enum MovementState
    {
        Idle, Moving, Rolling, Locked
    }

    public enum CombatState
    {
        Idle, Shooting, Casting, Locked
    }

    public enum GeneralState
    {
        Alive, Dead, Stunned
    }
}

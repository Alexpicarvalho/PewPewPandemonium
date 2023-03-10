
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAiT : MonoBehaviour
{

    //public PlayerCombatHandler combatHandler;
    [SerializeField] Transform _hand;
    [SerializeField] Transform _firepoint;
    public GunSO _gunRef;

    public NavMeshAgent agent;

    public LayerMask whatIsGround, whatIsPlayer;

    private enum State { Idle, Shooting };
    private State currentState = State.Idle;
    private GunSO _gun;
    [SerializeField] private float bossScale = 2f;

    //Looking for player
    public float sightRange, attackRange;
    List<GameObject> playersInRange = new List<GameObject>();
    GameObject currentTarget;
    float distanceToCurrentTarget;
    List<GameObject> playersInRangeLastFrame = new List<GameObject>();
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States

    public bool playerInSightRange, playerInAttackRange;

    //animator

    private Animator animator;




    private void Start()
    {
        animator = GetComponent<Animator>();
        _gun = Instantiate(_gunRef);
        agent = GetComponent<NavMeshAgent>();
        _gun.PlaceInHand(_hand);
        _gun.SetWeaponValues(_firepoint);
        timeBetweenAttacks = _gun._timeBetweenShots;
        transform.localScale *= bossScale;
    }

    private void Update()
    {
        playersInRange.Clear();
        currentTarget = null;
        _gun.UpdateWeaponStatus();

        //Look for players nearby
        CheckForPlayers();
        PickTarget();

        if (currentTarget!= null) transform.LookAt(currentTarget.transform);
     
        
        if (CheckIfTargetInAttackRange() && currentTarget != null) AttackPlayer(State.Shooting);

    }

    private bool CheckIfTargetInAttackRange()
    {
        if (distanceToCurrentTarget <= attackRange) return true;
        else return false;
    }

    private void CheckForPlayers()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayer);

        foreach (var collider in colliders)
        {
            GameObject target = collider.gameObject;
            if (!playersInRange.Contains(target)) playersInRange.Add(target);
        }
    }

    private void PickTarget()
    {
        if (playersInRange.Count == 0) return;

        float[] distances = new float[playersInRange.Count];
        int index = 0;
        foreach (var player in playersInRange)
        {
            distances[index] = Vector3.Distance(transform.position, player.transform.position);
            index++;
        }
        int playerIndex = Array.IndexOf(distances, distances.Min());
        currentTarget = playersInRange[playerIndex];
        distanceToCurrentTarget = distances[playerIndex];
    }


    //private void ChasePlayer()
    //{
    //    agent.SetDestination(player.position);
    //}

    private void AttackPlayer(State state)
    {
        //   agent.SetDestination(transform.position);
        currentState = state;
       

        switch (state)
        {
            case State.Idle:
                // Do nothing
                break;

            case State.Shooting:
                // Shoot at player
                if (!alreadyAttacked)
                {
                    CallSkill();
                    _gun.NormalShoot();
                    ResetAttack();
                    //else StartCoroutine(combatHandler.ReadyNextShot());
                }
                break;
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void CallSkill()
    {
        if (_gun._weaponSkill._skillState != WeaponSkillSO.SkillState.Ready) return;

        animator.SetTrigger(_gun._weaponSkill._animatorTrigger);
        _gun._weaponSkill._skillState = WeaponSkillSO.SkillState.Casting;
    }

    public void StartCastingVFX()
    {
        _gun._weaponSkill.StartCastingVFX();
    }
    public void ExecuteSkill()
    {
        _gun.CastSkill();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.5f);
        Gizmos.DrawSphere(transform.position, sightRange);
    }
}

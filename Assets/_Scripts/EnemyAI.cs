using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public PlayerCombatHandler combatHandler;
    [SerializeField] Transform _hand;
    [HideInInspector] public GunSO _gun;
    public GunSO _weaponSlot2;
    bool _shotReady = true;
    float _timeBetweenShots;
    [HideInInspector] public bool _reloading = false;
    public float detectionRange = 10f;
    public float shootingRange = 5f;
    public float peekDuration = 2f;
    public float peekInterval = 5f;

    public Transform player;
    private NavMeshAgent agent;
    private float peekTimer = 0f;
    private bool isPeeking = false;
    private bool isShooting = false;
    private Transform currentObstacle;

    private enum State { Idle, Hiding, Shooting };
    private State currentState = State.Idle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _gun = _weaponSlot2;
        combatHandler.SetupWeapon(_weaponSlot2);
        _timeBetweenShots = _gun._timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is within detection range
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            // Set state to hiding
          //  currentState = State.Hiding;

            // Check if there is an obstacle between enemy and player
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, player.position - transform.position, out hitInfo, detectionRange))
            {
                if (hitInfo.collider.CompareTag("Obstacle"))
                {
                    // Move to the obstacle
                    currentObstacle = hitInfo.collider.transform;
                    agent.SetDestination(currentObstacle.position);
                }
                else
                {
                    // Move towards the player
                    currentObstacle = null;
                    agent.SetDestination(player.position);
                }
            }
            else
            {
                // Move towards the player
                currentObstacle = null;
                agent.SetDestination(player.position);
            }

            // Check if player is within shooting range
            if (Vector3.Distance(transform.position, player.position) <= shootingRange)
            {
                // Set state to shooting
                currentState = State.Shooting;
            }
        }
        else
        {
            // Set state to idle
            currentState = State.Idle;
        }
        // Perform state behavior
        switch (currentState)
        {
            case State.Idle:
                // Do nothing
                break;

            case State.Hiding:
                // Peek out from behind obstacle
                if (currentObstacle != null && !isPeeking)
                {
                    StartCoroutine(Peek());
                }
                break;

            case State.Shooting:
                // Shoot at player
                if (!isShooting)
                {
                    _gun.NormalShoot();
                    if (_gun._bulletsInMag <= 0) StartCoroutine(combatHandler.Reload());
                    //else StartCoroutine(combatHandler.ReadyNextShot());
                }
                break;
        }
    }

    IEnumerator Peek()
    {
        isPeeking = true;
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = currentObstacle.position + (currentObstacle.forward * 0.5f);
        Quaternion originalRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(player.position - currentObstacle.position);

        float timer = 0f;
        while (timer < peekDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, timer / peekDuration);
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, timer / peekDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        yield return new WaitForSeconds(peekInterval);

        isPeeking = false;
    }
}

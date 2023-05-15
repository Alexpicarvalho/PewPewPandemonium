using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDamage : MonoBehaviour
{
    public float damagePerTick = 10f;  // Amount of damage to apply per tick
    public float tickInterval = 1f;    // Time interval between each tick
    public float exitDelay = 1f;       // Delay before the damage starts after exiting the zone

    private float lastTickTime;        // Time of the last tick
    private bool isPlayerInZone;       // Flag to track player's zone status
    private General_Stats player;
    private Damage damage;

    private void Start()
    {
        damage = new Damage(damagePerTick);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            lastTickTime = Time.time + exitDelay;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }

    private void Update()
    {
        if (isPlayerInZone && Time.time >= lastTickTime + tickInterval)
        {
            lastTickTime = Time.time;
            ApplyDamageToPlayer();
        }
    }

    private void ApplyDamageToPlayer()
    {
        //Damage the player [TickDamage] amount
        player.HandleHit(damage); 
    }
}

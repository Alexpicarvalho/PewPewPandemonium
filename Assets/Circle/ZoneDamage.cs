using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class ZoneDamage : NetworkBehaviour/*, IPlayerJoined*/
{
    public static ZoneDamage Instance;

    [SerializeField] LayerMask _playerLayer;

    public float damagePerTick = 10f;  // Amount of damage to apply per tick
    public float tickInterval = 1f;    // Time interval between each tick
    public float exitDelay = 1f;       // Delay before the damage starts after exiting the zone
    public int circleLevel = 1;

    private float lastTickTime;        // Time of the last tick
    private bool isPlayerInZone;       // Flag to track player's zone status
    private General_Stats player;
    private Damage damage;


    //Para mais tarde

    List<Transform> _playersInside = new List<Transform>(); // Igual à lista de player In Game no inicio
    List<Transform> _playersOutside = new List<Transform>();


    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        else Instance = this;
    }

    private void Start()
    {
        //damage = new Damage(damagePerTick);


    }

    public void AddPlayerInside(Transform player)
    {

        if (!_playersInside.Contains(player)) _playersInside.Add(player);
    }



    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;



        Debug.Log("Player Entrou");
        if (!_playersInside.Contains(other.transform)) _playersInside.Add(other.transform);
        if (_playersOutside.Contains(other.transform)) _playersOutside.Remove(other.transform);


        //if (other.CompareTag("Player"))
        //{
        //    isPlayerInZone = true;
        //    lastTickTime = Time.time + exitDelay;
        //}
    }

    private void OnTriggerExit(Collider other)
    {

        if (!other.CompareTag("Player")) return;

        Debug.Log("Player Saiu");
        if (_playersInside.Contains(other.transform))_playersInside.Remove(other.transform);
        if (!_playersOutside.Contains(other.transform)) _playersOutside.Add(other.transform);


        //if (other.CompareTag("Player"))
        //{
        //    isPlayerInZone = false;
        //}
    }

    private void Update()
    {
        lastTickTime += Runner.DeltaTime;

        if (lastTickTime > tickInterval)
        {
            lastTickTime = 0;
            DamagePlayersOutside();
        }

        //if (isPlayerInZone && Time.time >= lastTickTime + tickInterval)
        //{
        //    lastTickTime = Time.time;
        //    ApplyDamageToPlayer();
        //}
    }

    private void DamagePlayersOutside()
    {
        foreach (var player in _playersOutside)
        {
            player.GetComponent<IHitable>().HandleHit(new Damage(damagePerTick * circleLevel));
        }
    }

    private void ApplyDamageToPlayer()
    {
        //Damage the player [TickDamage] amount
        player.HandleHit(damage);
    }

    //public void PlayerJoined(PlayerRef player)
    //{
    //   player.
    //}
}

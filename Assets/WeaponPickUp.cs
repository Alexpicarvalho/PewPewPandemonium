using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponPickUp : Pickup
{
    [SerializeField] public GunSO _weaponToGive;
    [SerializeField] private float _startThrowForce = 100f;
    GunSO _gun;
    Rigidbody _rb;

    public override void Start()
    {
        base.Start();
        _gun = Instantiate(_weaponToGive);
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = false;
        Vector3 temp = (Vector3.up + RandomDirectionalVector() * (Random.Range(0, 2) * 2 - 1)) * _startThrowForce;
        _rb.AddForce(temp);
        Debug.Log("Added force" + temp);
    }

    private Vector3 RandomDirectionalVector()
    {
        Vector3 returnVec = Random.onUnitSphere;

        return returnVec;
    }

    public virtual void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(_center.position, _pickUpRange);
        foreach (var collider in colliders)
        {
            var possiblePlayer = collider.transform.GetComponent<PlayerCombatHandler>();
            if (Input.GetKeyDown(KeyCode.F))
            {
                foreach (var _anyPlayer in _playersInRange)
                {
                    if (_anyPlayer.transform == collider.transform)
                    {
                        _anyPlayer.ReceiveWeapon(_gun);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        var player = other.transform.GetComponent<PlayerCombatHandler>();
        if (player == null || !_canPickUp) return;
        else if (!_playersInRange.Contains(player)) _playersInRange.Add(player);


    }
    private void OnTriggerExit(Collider other)
    {
        var player = other.transform.GetComponent<PlayerCombatHandler>();
        if (player == null) return;
        else if (_playersInRange.Contains(player))
        {
            _playersInRange.Remove(player);
        }
    }
}

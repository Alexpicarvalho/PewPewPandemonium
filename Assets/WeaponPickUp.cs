using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : Pickup
{
    [SerializeField] public GunSO _weaponToGive;
    GunSO _gun;

    public override void Start()
    {
        base.Start();
        _gun = Instantiate(_weaponToGive);
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
        else if(!_playersInRange.Contains(player))_playersInRange.Add(player);

        
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

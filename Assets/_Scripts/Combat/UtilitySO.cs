using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[CreateAssetMenu(menuName = "Weapons/Utility", fileName = "Utility")]
public class UtilitySO : Item
{
    [Header("General Properties")]
    public float _cooldown;
    public string _animatorTrigger;
    [SerializeField] GameObject _dropPrefab;

    [Header("References")]
    protected Transform _player;

    [Header("Runtime Propeties")]
    public float _timeSinceLastUse;
    public bool _onCooldown = false;
    protected NetworkBehaviour _runnerNetworkBehaviour;
    protected Transform _firePoint;

    [Header("Optional Properties")]
    protected Object_ID _ownerID;

    public virtual void SetupUtility(Transform player, Transform firePoint)
    {
        _player = player;
        _firePoint = firePoint;
        _runnerNetworkBehaviour = player.GetComponent<NetworkBehaviour>();
        _ownerID = player.GetComponent<Object_ID>();
        _timeSinceLastUse = _cooldown;
    }

    public virtual void UpdateUtilityStatus()
    {
        if (!_onCooldown) return;
        _timeSinceLastUse += Time.deltaTime;
        if (_timeSinceLastUse >= _cooldown) _onCooldown = false;
    }

    public virtual void Use() 
    {
    }

    public virtual void DropUtility()
    {
        if (!_dropPrefab) return;

        var drop = _runnerNetworkBehaviour.Runner.Spawn(_dropPrefab, _firePoint.position + Vector3.up, Quaternion.identity);
        //var dropRB = drop.GetComponent<Rigidbody>();
        //drop.GetComponent<WeaponPickUp>()._weaponToGive = this;
    }

}

public enum UtilityType { Spawnable, Useable }
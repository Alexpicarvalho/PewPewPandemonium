using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;


[CreateAssetMenu(menuName = "Weapons/GunSO", fileName = "Gun")]
public class GunSO : ScriptableObject
{
    //Properties

    [Header("Physical/Visual Properties")]
    [SerializeField] private GameObject _weaponGO;
    [SerializeField] private GameObject _bulletGO;
    [SerializeField] private GameObject _muzzleFlash;

    [Header("Individual Attributes")]
    [SerializeField] private WeaponSkillSO _weaponSkill;
    [SerializeField] private float _damageMultiplier = 1;
    [SerializeField] private int _bulletsPerMinute;
    [SerializeField] private int _magazineSize;
    [SerializeField] public float _reloadTime;
    [SerializeField] private float _critDamageMultiplier = 2;
    [SerializeField] [Range(0, 1)] private float _weaponWeight;
    [SerializeField] [Range(-1, 1)] private float _weaponSights;

    [Header("RunTime Properties")]
    [SerializeField] public int _bulletsInMag;
    private Transform _firePoint;

    [Header("Hand Placement")]
    [SerializeField] private Vector3 _positionInHand;
    [SerializeField] private Quaternion _rotationInHand;

    [Header("Hidden Variables")]
    [HideInInspector] public bool _shotReady;
    [HideInInspector] public float _timeBetweenShots;


    //Methods

    public void SetWeaponValues(PlayerCombatHandler _combatHandler)
    {
        _timeBetweenShots = 60.0f/_bulletsPerMinute;
        _shotReady = true;
        Reload();
        if (_firePoint == null) Debug.LogError("NO FIREPOINT IN WEAPON");
    }

    public virtual void PlaceInHand(Transform parentBone)
    {
        GameObject weapon = Instantiate(_weaponGO);
        _firePoint = weapon.transform.GetChild(0);
        weapon.transform.parent = parentBone;
        weapon.transform.localPosition = _positionInHand;
        weapon.transform.localRotation = _rotationInHand;
    }
    public virtual void NormalShoot()
    {
        if (!_shotReady) return;
        Debug.Log("Shot a shot");
        _shotReady = false;
        _bulletsInMag--;
        Instantiate(_muzzleFlash, _firePoint.position, Quaternion.identity);
        var bullet = Instantiate(_bulletGO, _firePoint.position, Quaternion.LookRotation(_firePoint.forward));
        // Get bullet script and pass necessary variables
    }

    public virtual void CastSkill()
    {
        // Cast Skill In Weapon Skill Scriptable Object
    }

    public virtual void Reload()
    {
        _bulletsInMag = _magazineSize;
    }
}

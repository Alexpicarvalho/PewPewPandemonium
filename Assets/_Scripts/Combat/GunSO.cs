using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(menuName = "Weapons/GunSO", fileName = "Gun")]
public class GunSO : ScriptableObject
{
    //Properties

    [Header("Physical/Visual Properties")]
    [SerializeField] private GameObject _weaponGO;
    [SerializeField] private GameObject _bulletGO;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] public Texture _weaponIcon;
    [SerializeField] private GameObject _groundPickUpPrefab;

    [Header("Optional visual Properties (Must be child 2)")]
    [SerializeField] private GameObject _visualEffect;
    [SerializeField] private ExtraEffect _extraEffectType;
    [SerializeField] public float _extraEffectTimeToShutOff;

    [Header("Individual Attributes")]
    [SerializeField] public FireingType _fireingType;
    [SerializeField] public WeaponSkillSO _weaponSkillRef;
    [SerializeField] public WeaponTier _weaponTier;
    [SerializeField] public float _skillDamage;
    [SerializeField] private float _damageMultiplier = 1;
    [SerializeField] private int _bulletsPerMinute;
    [SerializeField] public int _magazineSize;
    [SerializeField] public float _reloadTime;
    [SerializeField] private float _critDamageMultiplier = 2;
    [SerializeField] [Range(0, 1)] private float _weaponWeight;
    [SerializeField] [Range(-1, 1)] private float _weaponSights;
    [SerializeField] [Range(0, 1)] private float _weaponInaccuracy;
    [SerializeField] public float _maxInaccuracy = 10;
    [SerializeField] public float _maxDisplacement = .1f;

    [Header("RunTime Properties")]
    [SerializeField] public int _bulletsInMag;
    [SerializeField] public ShootingStatus _currentShootingStatus;
    private Transform _firePoint;
    [HideInInspector] public GameObject _weaponClone;
    [HideInInspector] public GameObject _vfxClone;
    public WeaponSkillSO _weaponSkill;

    [Header("Hand Placement")]
    [SerializeField] private Vector3 _positionInHand;
    [SerializeField] private Quaternion _rotationInHand;

    [Header("Animators and Animations")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator _weaponAnimator;

    [Header("Hidden Variables")]
    //[HideInInspector] public bool _shotReady;
    [HideInInspector] public float _timeBetweenShots;
    [HideInInspector] private float _timeSinceLastShot;
    [HideInInspector] private float _timeSinceReloadStart;

    //Enums

    public enum FireingType
    {
        Automatic, SemiAutomatic, Manual
    }
    public enum ExtraEffect
    {
        Toggle, OneUse
    }
    public enum ShootingStatus
    {
        ShotReady, BetweenShots, Reloading
    }

    //Methods

    public void SetWeaponValues(Transform firePoint)
    {
        _firePoint = firePoint;
        _weaponSkill = Instantiate(_weaponSkillRef);
        _weaponSkill.SetSkillValues(_firePoint, _damageMultiplier);
        _weaponSkill._damage = _skillDamage;
        _timeBetweenShots = 60.0f / _bulletsPerMinute;
        _currentShootingStatus = ShootingStatus.ShotReady;
        if (_visualEffect)
        {
            _vfxClone = Instantiate(_visualEffect, _firePoint.transform.position, Quaternion.LookRotation(_firePoint.forward));
            _vfxClone.transform.parent = _firePoint;
            _vfxClone.SetActive(false);
        }
        Reload();
        if (_firePoint == null) Debug.LogError("NO FIREPOINT IN WEAPON");
    }

    public virtual void PlaceInHand(Transform parentBone)
    {
        _weaponClone = Instantiate(_weaponGO);
        //_firePoint = _weaponClone.transform.GetChild(0);
        _weaponClone.transform.parent = parentBone;
        _weaponClone.transform.localPosition = _positionInHand;
        _weaponClone.transform.localRotation = _rotationInHand;
    }

    public void UpdateWeaponStatus()
    {
        _weaponSkill.UpdateSkillStatus();
        if (_currentShootingStatus != ShootingStatus.Reloading && _bulletsInMag <= 0)
        {
            _currentShootingStatus = ShootingStatus.Reloading;
            /*This makes it so when magazine is empty the player immediately Reloads.
            If we want it to only happen when it's empty and player tries to shoot, we must swap to this state
            in NormalShoot()
            */
        }
        if(_currentShootingStatus == ShootingStatus.Reloading)
        {
            _timeSinceReloadStart += Time.deltaTime;
            if(_timeSinceReloadStart >= _reloadTime) Reload();
        }

        if (_currentShootingStatus == ShootingStatus.BetweenShots)
        {
            _timeSinceLastShot += Time.deltaTime;
            if (_timeSinceLastShot >= _timeBetweenShots) _currentShootingStatus = ShootingStatus.ShotReady;
        }

    }

    public GameObject GetSwapped()
    {
        return _weaponClone;
    }

    public virtual void NormalShoot()
    {
        if (_currentShootingStatus != ShootingStatus.ShotReady || _firePoint == null) return;

        _currentShootingStatus = ShootingStatus.BetweenShots;
        _bulletsInMag--;
        _timeSinceLastShot = 0;

        Instantiate(_muzzleFlash, _firePoint.position, Quaternion.identity);
        var bullet = Instantiate(_bulletGO, _firePoint.position, Quaternion.LookRotation(_firePoint.forward));
        bullet.transform.Translate(GetDisplacement(), 0, GetDisplacement());
        bullet.transform.Rotate(0, GetInaccuracy(), 0, Space.Self);
        //PlayExtraEffect();
        // Get bullet script and pass necessary variables
    }

    private void PlayExtraEffect()
    {
        switch (_extraEffectType)
        {
            case ExtraEffect.Toggle:
                break;
            case ExtraEffect.OneUse:
                Debug.Log("Hellow?");
                _vfxClone.SetActive(true);
                break;
            default:
                break;
        }
    }

    public virtual void CastSkill()
    {
        // Cast Skill In Weapon Skill Scriptable Object
        _weaponSkill.ExecuteSpell();
    }

    public virtual void Reload()
    {
        _bulletsInMag = _magazineSize;
        _timeSinceReloadStart = 0;
        _currentShootingStatus = ShootingStatus.ShotReady;
    }
    public virtual void ReloadCanceled()
    {
        _timeSinceReloadStart = 0;
    }

    float GetInaccuracy()
    {
        return Random.Range(0, _maxInaccuracy) * _weaponInaccuracy;
    }

    float GetDisplacement()
    {
        return Random.Range(-_maxDisplacement, _maxDisplacement) * _weaponInaccuracy;
    }

    public void DropWeapon()
    {
        Destroy(_weaponClone);
        Destroy(_vfxClone);
        _weaponSkill.DestroySpell();
        if (!_groundPickUpPrefab) return;

        var drop = Instantiate(_groundPickUpPrefab, _firePoint.position + Vector3.up, Quaternion.identity);
        //var dropRB = drop.GetComponent<Rigidbody>();
        drop.GetComponent<WeaponPickUp>()._weaponToGive = this;
        Debug.LogWarning("Successfully pooped weapon");
        //dropRB.isKinematic = false;
        //dropRB.AddForce(Vector3.up + Vector3.forward * 100f);


    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Fusion;


[CreateAssetMenu(menuName = "Weapons/GunSO", fileName = "Gun")]
public class GunSO : Item
{

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
    [SerializeField] public string _shootingAnimTrigger;
    [SerializeField] public FireingType _fireingType;
    [SerializeField] public WeaponSkillSO _weaponSkillRef;
    [SerializeField] public WeaponTier _weaponTier;
    //[SerializeField] public float _skillDamage;
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

    [Header("Special Properties")]
    [SerializeField] bool _coneShooting = false;
    [SerializeField] private int _bulletsFired = 1;
    [SerializeField] private float _fireingConeAngle;


    [Header("Audio")]
    [SerializeField] SoundData _soundData;
    private AudioSource _audioSource;

    [Header("RunTime Properties")]
    [SerializeField] public int _bulletsInMag;
    [SerializeField] public ShootingStatus _currentShootingStatus;
    private Transform _firePoint;
    [HideInInspector] public GameObject _weaponClone;
    [HideInInspector] public GameObject _vfxClone;
    public WeaponSkillSO _weaponSkill;
    public int _ownerID;
    protected NetworkBehaviour _runnerNetworkbehaviour;

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

    public void SetWeaponValues(Transform firePoint, Object_ID parentID = null)
    {
        _firePoint = firePoint;
        _runnerNetworkbehaviour = parentID.GetComponentInParent<NetworkBehaviour>();
        _weaponSkill = Instantiate(_weaponSkillRef);
        _weaponSkill.SetSkillValues(_firePoint, _damageMultiplier, parentID);
        _timeBetweenShots = 60.0f / _bulletsPerMinute;
        _currentShootingStatus = ShootingStatus.ShotReady;
        _audioSource = _firePoint.GetComponent<AudioSource>();
        if (parentID) _ownerID = parentID.my_ID;
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

        if(_coneShooting) { ConeShoot(); return; }
        else ShootForward();
        
    }
    private void ShootForward()
    {
        _currentShootingStatus = ShootingStatus.BetweenShots;
        _bulletsInMag--;
        _timeSinceLastShot = 0;

        if (_muzzleFlash) _runnerNetworkbehaviour.Runner.Spawn(_muzzleFlash, _firePoint.position, Quaternion.identity);
        var bullet = _runnerNetworkbehaviour.Runner.Spawn(_bulletGO, _firePoint.position, Quaternion.LookRotation(_firePoint.forward));
        bullet.transform.Translate(GetDisplacement(), 0, GetDisplacement());
        bullet.transform.Rotate(0, GetInaccuracy(), 0, Space.Self);
        //bullet.GetComponent<Damager>().SetDamage();
        if (_soundData != null) _audioSource.PlayOneShot(_soundData.GetRandomSound(), _soundData.GetClipVolume());
        //PlayExtraEffect();
        // Get bullet script and pass necessary variables
    }

    private void ShootEffects()
    {
        if (_muzzleFlash) _runnerNetworkbehaviour.Runner.Spawn(_muzzleFlash, _firePoint.position, Quaternion.identity);
        _currentShootingStatus = ShootingStatus.BetweenShots;
        _bulletsInMag--;
        _timeSinceLastShot = 0;
        if (_soundData) _audioSource.PlayOneShot(_soundData.GetRandomSound(), _soundData.GetClipVolume());
    }

    private void ConeShoot()
    {
        if (_bulletsFired % 2 == 1) ShootForward();
        else
        {
            _runnerNetworkbehaviour.Runner.Spawn(_muzzleFlash, _firePoint.position, Quaternion.identity);
            _currentShootingStatus = ShootingStatus.BetweenShots;
            _bulletsInMag--;
            _timeSinceLastShot = 0;
            //if (_soundData) _audioSource.PlayOneShot(_soundData.GetRandomSound(), _soundData.GetClipVolume());
        }
        ShootSideBullets();
    }

    private void ShootSideBullets()
    {  
        int _bulletsToSpawn = _bulletsFired - 1;
        for (int i = 1; i <= _bulletsToSpawn / 2; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis((_fireingConeAngle / _bulletsToSpawn) * i, Vector3.up);
            Vector3 direction = (rotation * _firePoint.forward).normalized;
   
            var bullet = _runnerNetworkbehaviour.Runner.Spawn(_bulletGO, _firePoint.position, Quaternion.LookRotation(direction));
            bullet.transform.Translate(GetDisplacement(), 0, GetDisplacement());
            bullet.transform.Rotate(0, GetInaccuracy(), 0, Space.Self);
            bullet.GetComponent<Damager>().SetDamage();
        }
        for (int i = 1; i <= _bulletsToSpawn / 2; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis((_fireingConeAngle / _bulletsToSpawn) * i, Vector3.up);
            Vector3 direction = (Quaternion.Inverse(rotation) * _firePoint.forward).normalized;

            var bullet = _runnerNetworkbehaviour.Runner.Spawn(_bulletGO, _firePoint.position, Quaternion.LookRotation(direction));
            bullet.transform.Translate(GetDisplacement(), 0, GetDisplacement());
            bullet.transform.Rotate(0, GetInaccuracy(), 0, Space.Self);
            bullet.GetComponent<Damager>().SetDamage();
        }
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

    public virtual void CastSkill(Vector3 target = default)
    {
        // Cast Skill In Weapon Skill Scriptable Object
        _weaponSkill.ExecuteSpell(target);
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

        var drop = _runnerNetworkbehaviour.Runner.Spawn(_groundPickUpPrefab, _firePoint.position + Vector3.up, Quaternion.identity);
        //var dropRB = drop.GetComponent<Rigidbody>();
        drop.GetComponent<WeaponPickUp>()._weaponToGive = this;
        Debug.LogWarning("Successfully pooped weapon");
        //dropRB.isKinematic = false;
        //dropRB.AddForce(Vector3.up + Vector3.forward * 100f);


    }
}

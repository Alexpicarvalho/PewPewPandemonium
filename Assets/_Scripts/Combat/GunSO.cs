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
    [SerializeField] public Texture _weaponIcon;
    [SerializeField] private GameObject _groundPickUpPrefab;

    [Header("Optional visual Properties (Must be child 2)")]
    [SerializeField] private GameObject _visualEffect;
    [SerializeField] private ExtraEffect _extraEffectType;
    [SerializeField] public float _extraEffectTimeToShutOff;

    [Header("Individual Attributes")]
    [SerializeField] public FireingType _fireingType;
    [SerializeField] public WeaponSkillSO _weaponSkill;
    [SerializeField] public float _skillDamage;
    [SerializeField] private float _damageMultiplier = 1;
    [SerializeField] private int _bulletsPerMinute;
    [SerializeField] public int _magazineSize;
    [SerializeField] public float _reloadTime;
    [SerializeField] private float _critDamageMultiplier = 2;
    [SerializeField][Range(0, 1)] private float _weaponWeight;
    [SerializeField][Range(-1, 1)] private float _weaponSights;

    [Header("RunTime Properties")]
    [SerializeField] public int _bulletsInMag;
    private Transform _firePoint;
    [HideInInspector] public GameObject _weaponClone;
    [HideInInspector] public GameObject _vfxClone;

    [Header("Hand Placement")]
    [SerializeField] private Vector3 _positionInHand;
    [SerializeField] private Quaternion _rotationInHand;

    [Header("Animators and Animations")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator _weaponAnimator;

    [Header("Hidden Variables")]
    [HideInInspector] public bool _shotReady;
    [HideInInspector] public float _timeBetweenShots;

    //Enums

    public enum FireingType
    {
        Automatic, SemiAutomatic, Manual
    }
    public enum ExtraEffect
    {
        Toggle, OneUse
    }

    //Methods

    public void SetWeaponValues(Transform firePoint)
    {
        _firePoint = firePoint;
        _weaponSkill.SetSkillValues(_firePoint);
        _weaponSkill._damage = _skillDamage;
        _timeBetweenShots = 60.0f / _bulletsPerMinute;
        _shotReady = true;
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
    public GameObject GetSwapped()
    {
        return _weaponClone;
    }

    public virtual void NormalShoot()
    {
        if (!_shotReady || _firePoint == null) return;
        
        _shotReady = false;
        _bulletsInMag--;
        
        Instantiate(_muzzleFlash, _firePoint.position, Quaternion.identity);
        var bullet = Instantiate(_bulletGO, _firePoint.position, Quaternion.LookRotation(_firePoint.forward));
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
    }

    public void DestroyWeapon()
    {
        Destroy(_weaponClone);
    }
}

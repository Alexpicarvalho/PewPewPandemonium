using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;


public class PlayerCombatHandler : MonoBehaviour
{
    [SerializeField] Transform _hand;
    [SerializeField] Transform _firePoint;
    [SerializeField] GunSO _startingWeapon1;
    [SerializeField] GunSO _startingWeapon2;
    public GunSO _weaponSlot1;
    public GunSO _weaponSlot2;
    [HideInInspector] public GunSO _gun;
    bool _shotReady = true;
    float _timeBetweenShots;
    [HideInInspector] public bool _reloading = false;
    private Animator _animator;

    [Header("Weapon Swapping")]
    [SerializeField] private float _swapCooldown = .6f;
    [SerializeField] private float _swapEffectDuration;
    private bool _swapReady = true;
    [SerializeField] Material _dissolveMat;
    
    
    //Temporary Knife Combat
    void KnifeAttack() 
    {
        _animator.Play("Slash");
    }

    
    private void Awake()
    {
        _swapReady = true;
        _animator = GetComponent<Animator>();
        //TEMP 
        if (_startingWeapon1)
        {
            _weaponSlot1 = Instantiate(_startingWeapon1);
            SetupWeapon(_weaponSlot1);
        }
        if (_startingWeapon2)
        {
            _weaponSlot2 = Instantiate(_startingWeapon2);
            SetupWeapon(_weaponSlot2);
        }
        _gun = _weaponSlot2;
        SwapWeapons();
        _timeBetweenShots = _gun._timeBetweenShots;
    }

    private void Update()
    {
        //Do we want the cooldown to go down only if the gun is equipped?
        //_gun.UpdateWeaponStatus();

        //Do we want the cooldown to keep going down wether or not the gun is equipped?
        _weaponSlot1?.UpdateWeaponStatus();
        _weaponSlot2?.UpdateWeaponStatus();

        if (Input.GetKeyDown(KeyCode.V))
        {
            KnifeAttack();
        }


        if (Input.GetButton("Fire1"))
        {
            _gun.NormalShoot();

        }

        if (Input.GetButton("Fire2"))
        {
            _gun._weaponSkill.ShowSkillIndicator();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            CallSkill();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwapWeapons();
        }
        
    }

    public void ReceiveWeapon(GunSO _newGun)
    {
        //First We check if any slots are empty, in which case weapon goes there
        if (!_weaponSlot1)
        {
            Debug.LogWarning("1");
            _weaponSlot1 = Instantiate(_newGun);
            SetupWeapon(_weaponSlot1);
        }
        else if (!_weaponSlot2)
        {
            Debug.LogWarning("2");
            _weaponSlot2 = Instantiate(_newGun);
            SetupWeapon(_weaponSlot2);
        }

        //If there are no empty slots, we drop whichever gun we are holding and replace it with new one
        else
        {
            _gun.DropWeapon();
            if (_gun == _weaponSlot1)
            {
                Debug.LogWarning("3");
                _weaponSlot1 = Instantiate(_newGun);
                _gun = _weaponSlot1;
            }
            else
            {
                Debug.LogWarning("4");
                _weaponSlot2 = Instantiate(_newGun);
                _gun = _weaponSlot2;
            }
            SetupWeapon(_gun);
        }
        SwapWeapons();
    }

    public void SetupWeapon(GunSO newWeapon)
    {
        newWeapon.PlaceInHand(_hand);
        newWeapon.SetWeaponValues(_firePoint);
    }

    private void SwapWeapons()
    {
        if (!_weaponSlot1 || !_weaponSlot2 || !_swapReady) return; //If player doesn't have 2 weapons returns
        NextWeapon();
        //StartCoroutine(SwapVisualEffect());


    }

    private void NextWeapon()
    {
        _gun._weaponClone.SetActive(false);
        _swapReady = false;
        if (_gun == _weaponSlot1) _gun = _weaponSlot2;
        else _gun = _weaponSlot1;
        _timeBetweenShots = _gun._timeBetweenShots;
        _gun._weaponClone.SetActive(true);
        StartCoroutine(ReadySwap());
    }

    private void CallSkill()
    {
        if (_gun._weaponSkill._skillState != WeaponSkillSO.SkillState.Ready) return;
        _animator.SetTrigger(_gun._weaponSkill._animatorTrigger);
        Debug.LogWarning("Set it to cast");
        _gun._weaponSkill._skillState = WeaponSkillSO.SkillState.Casting;
    }

    public void StartCastingVFX()
    {
        _gun._weaponSkill.StartCastingVFX();
    }
    public void ExecuteSkill()
    {
        _gun.CastSkill();
    }

    //public IEnumerator ReadyNextShot()
    //{
    //    yield return new WaitForSeconds(_timeBetweenShots);
    //    _gun._shotReady = true;
    //    _shotReady = true;
    //}

    public IEnumerator ShutOffExtraEffect()
    {
        yield return new WaitForSeconds(_gun._extraEffectTimeToShutOff);
        if (_gun._vfxClone != null) _gun._vfxClone.SetActive(false);
    }

    public IEnumerator Reload()
    {
        _gun._currentShootingStatus = GunSO.ShootingStatus.Reloading;
        //_shotReady = false;
        _reloading = true;

        yield return new WaitForSeconds(_gun._reloadTime);

        _gun.Reload();
        _reloading = false;
        _gun._currentShootingStatus = GunSO.ShootingStatus.ShotReady;
        //_shotReady = true;
        //_gun._shotReady = true;
    }

    public IEnumerator ReadySwap()
    {
        yield return new WaitForSeconds(_swapCooldown);
        _swapReady = true;
    }

    public IEnumerator SwapVisualEffect()
    {
        _shotReady = false;
        float startTime = Time.time;
        float time = 0;
        while (Time.time < startTime + _swapEffectDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / _swapEffectDuration);
            _dissolveMat.SetFloat("_Dissolve", Mathf.Lerp(0, 1, t));
            yield return null;
        }

        NextWeapon();

        //Show again
        startTime = Time.time;
        time = 0;
        while (Time.time < startTime + _swapEffectDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / _swapEffectDuration);
            _dissolveMat.SetFloat("_Dissolve", Mathf.Lerp(1, 0, t));
            yield return null;
        }
        _shotReady = true;
    }
}
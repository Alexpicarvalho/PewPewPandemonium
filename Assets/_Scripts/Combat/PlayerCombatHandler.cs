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
    [SerializeField ] Material _dissolveMat;

    private void Start()
    {
        _swapReady = true;
        _animator = GetComponent<Animator>();
        _gun = _weaponSlot2;
        //TEMP 
        SetupWeapon(_weaponSlot1);
        SetupWeapon(_weaponSlot2);
        SwapWeapons();
        _timeBetweenShots = _gun._timeBetweenShots;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && _shotReady)
        {
            //StopCoroutine(ShutOffExtraEffect());
            _gun.NormalShoot();
            _shotReady = false;
            if (_gun._bulletsInMag <= 0) StartCoroutine(Reload());
            else
            {
                StartCoroutine(ReadyNextShot());
                //StartCoroutine(ShutOffExtraEffect());
            }
        }
        Debug.Log(_swapReady);

        if (Input.GetButton("Fire2"))
        {
            _gun._weaponSkill.ShowSkillIndicator();
        }
        else if (Input.GetButtonUp("Fire2")) 
        {
            //_gun._weaponSkill.HideSkillIndicator();
            CallSkill(); 
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwapWeapons();
        }
    }

    public void ReceiveWeapon()
    {

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
        _animator.SetTrigger(_gun._weaponSkill._animatorTrigger);
    }

    public void StartCastingVFX()
    {
        _gun._weaponSkill.StartCastingVFX();
    }
    public void ExecuteSkill()
    {
        _gun.CastSkill();
    }

    public IEnumerator ReadyNextShot()
    {
        yield return new WaitForSeconds(_timeBetweenShots);
        _gun._shotReady = true;
        _shotReady = true;
    }

    public IEnumerator ShutOffExtraEffect()
    {
        yield return new WaitForSeconds(_gun._extraEffectTimeToShutOff);
        if (_gun._vfxClone != null) _gun._vfxClone.SetActive(false);
    }

    public IEnumerator Reload()
    {
        _gun._shotReady = false;
        _shotReady = false;
        _reloading = true;

        yield return new WaitForSeconds(_gun._reloadTime);

        _gun.Reload();
        _reloading = false;
        _shotReady = true;
        _gun._shotReady = true;
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
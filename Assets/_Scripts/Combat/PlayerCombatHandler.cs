using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;


public class PlayerCombatHandler : MonoBehaviour
{
    [SerializeField] Transform _hand;
    public GunSO _weaponSlot1;
    public GunSO _weaponSlot2;
    [HideInInspector] public GunSO _gun;
    bool _shotReady = true;
    float _timeBetweenShots;
    [HideInInspector] public bool _reloading = false;

    private void Start()
    {
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
            _gun.NormalShoot();
            _shotReady = false;  
            if (_gun._bulletsInMag <= 0) StartCoroutine(Reload());
            else StartCoroutine(ReadyNextShot());
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
        newWeapon.SetWeaponValues();
    }

    private void SwapWeapons()
    {
        if (!_weaponSlot1 || !_weaponSlot2) return; //If player doesn't have 2 weapons returns

        _gun._weaponClone.SetActive(false);

        if (_gun == _weaponSlot1) _gun = _weaponSlot2;
        else _gun = _weaponSlot1;
        _timeBetweenShots = _gun._timeBetweenShots;
        _gun._weaponClone.SetActive(true);
    }

    IEnumerator ReadyNextShot()
    {
        yield return new WaitForSeconds(_timeBetweenShots);
        _gun._shotReady = true;
        _shotReady = true;
    }

    IEnumerator Reload()
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
}
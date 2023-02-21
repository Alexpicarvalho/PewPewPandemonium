using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;


public class PlayerCombatHandler : MonoBehaviour
{
    [SerializeField] Transform _hand;
    public GunSO _gun;
    bool _shotReady = true;
    float _timeBetweenShots;

    private void Start()
    {
        //TEMP 
        _gun.PlaceInHand(_hand);
        _gun.SetWeaponValues(this);
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
        
    }


    IEnumerator ReadyNextShot()
    {
        yield return new WaitForSeconds(_timeBetweenShots);
        _gun._shotReady = true;
        _shotReady = true;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(_gun._reloadTime);
        _gun.Reload();
        _shotReady = true;
    }
}
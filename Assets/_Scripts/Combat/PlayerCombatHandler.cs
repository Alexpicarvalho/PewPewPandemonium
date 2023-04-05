using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;


public class PlayerCombatHandler : MonoBehaviour
{
    [Header("Starting Weapon")]
    [SerializeField] GunSO _startingWeapon;
    

    [Header("Requirements")]
    [SerializeField] Transform _hand;
    [SerializeField] Transform _firePoint;
    private Animator _animator;
    private Object_ID _id;

    [Header("Runtime Variables")]
    [HideInInspector] public bool _reloading = false;
    [HideInInspector] public GunSO _weaponSlot1;
    [HideInInspector] public GunSO _weaponSlot2;
    [HideInInspector] public GunSO _gun;
    /*[HideInInspector]*/ public GrenadeSO _grenade;
    private Vector3 _currentSkillTargetLocation;
    //[HideInInspector] public UtilitySO _utility;

    [Header("Weapon Swapping Properties")]
    [SerializeField] private float _swapCooldown = .6f;
    //[SerializeField] private float _swapEffectDuration;
    private bool _swapReady = true;
    //[SerializeField] Material _dissolveMat;

    [Header("Perk Related Properties")]
    private float _cooldownReduction;
    private float _reloadTimeReduction;

    //Temporary Knife Combat
    void KnifeAttack() 
    {
        _animator.Play("Slash");
    }

    
    private void Awake()
    {
        Debug.Log("ENTREI NO AWAKE");
        _id = GetComponent<Object_ID>();
        _swapReady = true;
        _animator = GetComponent<Animator>();
        //TEMP 
        if (_startingWeapon)
        {
            _weaponSlot2 = Instantiate(_startingWeapon);
            SetupWeapon(_weaponSlot2);
        }
        _gun = _weaponSlot2;
        SwapWeapons();
        _grenade = Instantiate(_grenade);
        _grenade.SetValues(_firePoint);
    }


    private void Update()
    {
        //Do we want the cooldown to go down only if the gun is equipped?
        //_gun.UpdateWeaponStatus();

        //Do we want the cooldown to keep going down wether or not the gun is equipped?
        _weaponSlot1?.UpdateWeaponStatus();
        _weaponSlot2?.UpdateWeaponStatus();
        _grenade.UpdateState();

        if (Input.GetKeyDown(KeyCode.V))
        {
            KnifeAttack();
        }


        if (Input.GetButton("Fire1"))
        {
            _gun.NormalShoot();
            _animator.SetTrigger(_gun._shootingAnimTrigger);

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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _grenade.Throw(MousePosition());
            //_grenade.EnableIndicator();
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
           // _grenade.DisableIndicator();
        }

        //_grenade.DrawProjection();

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
        newWeapon.SetWeaponValues(_firePoint, _id);
    }

    private void SwapWeapons()
    {
        if (!_weaponSlot1 || !_weaponSlot2 || !_swapReady) return; //If player doesn't have 2 weapons returns
        NextWeapon();
    }

    private void NextWeapon()
    {
        _gun._weaponClone.SetActive(false);
        _swapReady = false;
        if (_gun == _weaponSlot1) _gun = _weaponSlot2;
        else _gun = _weaponSlot1;
        _gun._weaponClone.SetActive(true);
        StartCoroutine(ReadySwap());
    }

    private void CallSkill()
    {
        if (_gun._weaponSkill._skillState != WeaponSkillSO.SkillState.Ready) return;

        _currentSkillTargetLocation = MousePosition();
        _gun._weaponSkill._skillState = WeaponSkillSO.SkillState.Casting;
        
        if (_gun._weaponSkill._animatorTrigger != "") _animator.SetTrigger(_gun._weaponSkill._animatorTrigger);
        else ExecuteSkill();

        
        
        Debug.LogWarning("Set it to cast");
        
    }

    public void StartCastingVFX()
    {
        _gun._weaponSkill.StartCastingVFX();
    }
    public void ExecuteSkill()
    {
        _gun.CastSkill(_currentSkillTargetLocation);
    }

    //public IEnumerator ReadyNextShot()
    //{
    //    yield return new WaitForSeconds(_timeBetweenShots);
    //    _gun._shotReady = true;
    //    _shotReady = true;
    //}

    #region Grenade Path Simulations 

    private Vector3 MousePosition()
    {
        Camera camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Plane plane = new Plane(Vector3.up, transform.position);

        if (Physics.Raycast(ray, out hit))
        {
            return new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
        }
        else return Vector3.zero;
    }




    IEnumerator GrenadeMovement(float initVel, float angle)
    {
        float t = 0;
        while (t < 100)
        {
            float x = initVel * t * Mathf.Cos(angle);
            float y = initVel * t * Mathf.Sin(angle) - (1f / 2f) * Physics.gravity.y * Mathf.Pow(t,2);
            transform.position = new Vector3(x,y,0);
            t += Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    #region Perk Related Methods

    public void UpdateCooldownReduction(float cdGain)
    {
        _cooldownReduction += cdGain;
    }
    public void UpdateReloadSpeed(float reloadSpeedGain)
    {
        _reloadTimeReduction += reloadSpeedGain;
    }

    #endregion

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

    //public IEnumerator SwapVisualEffect()
    //{
    //    float startTime = Time.time;
    //    float time = 0;
    //    while (Time.time < startTime + _swapEffectDuration)
    //    {
    //        time += Time.deltaTime;
    //        float t = Mathf.Clamp01(time / _swapEffectDuration);
    //        _dissolveMat.SetFloat("_Dissolve", Mathf.Lerp(0, 1, t));
    //        yield return null;
    //    }

    //    NextWeapon();

    //    //Show again
    //    startTime = Time.time;
    //    time = 0;
    //    while (Time.time < startTime + _swapEffectDuration)
    //    {
    //        time += Time.deltaTime;
    //        float t = Mathf.Clamp01(time / _swapEffectDuration);
    //        _dissolveMat.SetFloat("_Dissolve", Mathf.Lerp(1, 0, t));
    //        yield return null;
    //    }
    //}
}
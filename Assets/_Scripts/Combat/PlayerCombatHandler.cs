using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using Fusion;


public class PlayerCombatHandler : NetworkBehaviour
{
    [Header("Starting Weapon")]
    [SerializeField] GunSO _startingWeapon;


    [Header("Requirements")]
    [SerializeField] Transform _hand;
    [SerializeField] Transform _firePoint;
    private Animator _animator;
    private Object_ID _id;
    private PlayerFSM _playerState;
    public TempUIInfo _uiInfo;

    [Header("Runtime Variables")]
    [HideInInspector] public bool _reloading = false;
    /*[HideInInspector] */
    public GunSO _weaponSlot1;
    /*[HideInInspector]*/
    public GunSO _weaponSlot2;
    /*[HideInInspector]*/
    public GunSO _gun;
    public GunSO _offGun;
    /*[HideInInspector]*/
    public GrenadeSO _grenade;
    public UtilitySO _utility;
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


    public override void Spawned()
    {
        Debug.Log("Entrein Spawned PCH");

        _id = GetComponent<Object_ID>();
        _swapReady = true;
        _animator = GetComponent<Animator>();
        _playerState = GetComponent<PlayerFSM>();
        //TEMP 
        if (_startingWeapon)
        {
            _weaponSlot2 = Instantiate(_startingWeapon);
            SetupWeapon(_weaponSlot2);
        }
        else Debug.Log("No Starting Weapon");

        _gun = _weaponSlot2;
        SwapWeapons();
        _grenade = Instantiate(_grenade);
        _grenade.SetValues(_firePoint, _id);
        _utility = Instantiate(_utility);
        _utility.SetupUtility(transform, _firePoint);
    }


    private void Update()
    {
        //Do we want the cooldown to go down only if the gun is equipped?
        //_gun.UpdateWeaponStatus();

        //Do we want the cooldown to keep going down wether or not the gun is equipped?
        _weaponSlot1?.UpdateWeaponStatus();
        _weaponSlot2?.UpdateWeaponStatus();
        _grenade.UpdateState();
        _utility.UpdateUtilityStatus();

    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isMouse1ButtonPressed &&
                _playerState.combatState != PlayerFSM.CombatState.Locked &&
                _playerState.combatState != PlayerFSM.CombatState.Casting)
            {
                _gun.NormalShoot();
                _animator.SetTrigger(_gun._shootingAnimTrigger);
                _playerState.combatState = PlayerFSM.CombatState.Shooting;
            }
            _currentSkillTargetLocation = networkInputData.mousePosition;
            if (networkInputData.isMouse2ButtonPressed && _playerState.combatState != PlayerFSM.CombatState.Locked)
            {
                CallSkill();
            }

            if (networkInputData.isGrenadePressed && _playerState.combatState != PlayerFSM.CombatState.Locked && _playerState.combatState != PlayerFSM.CombatState.Casting)   
            {
                _grenade.Throw(networkInputData.mousePosition + Vector3.up * 0.1f);
            }
            if (networkInputData.isWeaponSwapPressed && _playerState.combatState != PlayerFSM.CombatState.Locked && _playerState.combatState != PlayerFSM.CombatState.Casting) SwapWeapons();

            if (networkInputData.isUtilityPressed && _playerState.combatState != PlayerFSM.CombatState.Locked && _playerState.combatState != PlayerFSM.CombatState.Casting) _utility.Use();

            if (networkInputData.isMeleePressed && _playerState.combatState != PlayerFSM.CombatState.Locked && _playerState.combatState != PlayerFSM.CombatState.Casting) KnifeAttack();

            if (networkInputData.isReloadPressed && _playerState.combatState != PlayerFSM.CombatState.Locked && _playerState.combatState != PlayerFSM.CombatState.Casting) _gun.ForceReload();

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
            //_gun = null;
            if (_gun == _weaponSlot1)
            {
                Debug.LogWarning("3");
                _weaponSlot1 = Instantiate(_newGun);
                _gun = _weaponSlot1;
                _offGun = _weaponSlot2;
            }
            else
            {
                Debug.LogWarning("4");
                _weaponSlot2 = Instantiate(_newGun);
                _gun = _weaponSlot2;
                _offGun = _weaponSlot1;
            }
            SetupWeapon(_gun);
        }
        //GetComponent<Hideable>().UpdateRenderers();
        SwapWeapons();
    }

    public void ReceiveGrenade(GrenadeSO _newGrenade)
    {
        if (_grenade != null) _grenade.DropGrenade();

        _grenade = Instantiate(_newGrenade);
        _grenade.SetValues(_firePoint, _id);
    }

    public void ReceiveUtility(UtilitySO _newUtility)
    {
        if (_grenade != null) _utility.DropUtility();

        _utility = Instantiate(_newUtility);
        _utility.SetupUtility(transform, _firePoint);
    }

    public void SetupWeapon(GunSO newWeapon)
    {

        newWeapon.SetWeaponValues(_firePoint, _id);
        //if (!HasStateAuthority) return;
        newWeapon.PlaceInHand(_hand);
    }

    private void SwapWeapons()
    {
        //if (!HasStateAuthority) return;
        if (!_weaponSlot1 || !_weaponSlot2 || !_swapReady) return; //If player doesn't have 2 weapons returns
        _uiInfo.CallSwap();
        NextWeapon();
    }

    private void NextWeapon()
    {
        //_gun._weaponClone.SetActive(false);
        _gun._weaponClone.gameObject.SetActive(false);
        _swapReady = false;
        if (_gun == _weaponSlot1)
        {
            _gun = _weaponSlot2;
            _offGun = _weaponSlot1;
        }
        else
        {
            _gun = _weaponSlot1;
            _offGun = _weaponSlot2;
        } 
        //_gun._weaponClone.SetActive(true);
        _gun._weaponClone.gameObject.SetActive(true);
        StartCoroutine(ReadySwap());
    }

    private void CallSkill()
    {
        if (_gun._weaponSkill._skillState != WeaponSkillSO.SkillState.Ready || _gun._noSkill) return;

        _gun._weaponSkill._skillState = WeaponSkillSO.SkillState.Casting;
        _playerState.TransitionState(PlayerFSM.CombatState.Casting);

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
        _playerState.TransitionState(PlayerFSM.CombatState.Idle);
    }

    public void OnDeathDropWeapons()
    {
        Debug.Log("DROPPING!");
        if (_weaponSlot2 != null) _gun.DropWeapon();
        if (_weaponSlot1 != null) _offGun.DropWeapon();

        //Destroy(_gun);
        //Destroy(_offGun);
        //Destroy(_weaponSlot1);
        //Destroy(_weaponSlot2);

        _gun = null;
        _offGun = null;
        _weaponSlot1 = null;
        _weaponSlot2 = null;

        //_weaponSlot1 = Instantiate(_startingWeapon);
        //SetupWeapon(_weaponSlot2);
        //_gun = _weaponSlot2;
        //SwapWeapons();


    }

    public void ResetStartWeapon()
    {
        if (GetComponent<General_Stats>()._respawnActive == false) return;

        //TEMP 
        if (_startingWeapon)
        {
            _weaponSlot2 = Instantiate(_startingWeapon);
            SetupWeapon(_weaponSlot2);
        }
        else Debug.Log("No Starting Weapon");

        _gun = Instantiate(_weaponSlot2);
        SwapWeapons();
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
            float y = initVel * t * Mathf.Sin(angle) - (1f / 2f) * Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = new Vector3(x, y, 0);
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
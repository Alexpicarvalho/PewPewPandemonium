using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityRandom = UnityEngine.Random;
using UnityEngine.UI;
using Fusion;


public class General_Stats : NetworkBehaviour, IHitable
{
    //General Properties
    [Header("General Properties")]
    [SerializeField] float _maxHP;
    [SerializeField] float _maxShield; // Changeable by armor
    [SerializeField] bool _isPushable;
    [Networked] NetworkBool _dead { get; set; }

    [Header("Start Properties")]
    [SerializeField] float _startingShield;
    //[SerializeField] float _startingHealth;

    //ReadOnly Variables
    public float MaxHP => _maxHP;
    public float CurrentHp => _currentHp;
    public float MaxShield => _maxShield;
    public float CurrentShield => _currentShield;
    public bool IsPushable => _isPushable;

    [field : Header("Runtime Properties")]
    [Networked] float _currentShield { get; set; }
    [field : SerializeField][Networked] float _currentHp { get; set; }

    [Header("Test")]
    [SerializeField] TextMeshProUGUI _hpText;
    [SerializeField] TextMeshProUGUI _shieldText;
    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _shieldSlider;
    [SerializeField] List<NetworkBehaviour> _scriptsToDisable;
    private Animator _anim;
    private PlayerFSM _playerState;
    private void Start()
    {
        _playerState = GetComponent<PlayerFSM>();
        _anim = GetComponent<Animator>();

        _currentHp = MaxHP;
        _currentShield = _startingShield;
        _dead = false;
        
        //Temp
        if (_shieldText != null) _shieldText.text = ((int)_currentShield).ToString() + " / " + MaxShield;
        if (_hpText != null) _hpText.text = ((int)_currentHp).ToString() + " / " + MaxHP;
        //if (_shieldSlider != null) _shieldSlider.value = _currentShield / MaxShield;
        //if (_hpSlider != null) _hpSlider.value = _currentHp/ MaxHP;     
    }

    public override void Render()
    {
        if (_shieldSlider != null) _shieldSlider.value = _currentShield / MaxShield;
        if (_hpSlider != null) _hpSlider.value = _currentHp / MaxHP;
    }

    public void HandleHit(Damage damage)
    {
        if (!HasStateAuthority) return;
        if (damage == null || _dead) return;
        if (_currentShield < damage._amount) OverflowDamage(Mathf.Abs(_currentShield - damage._amount));

        _currentShield -= damage._amount;
        if(_currentShield <= 0) _currentShield = 0;

        //TEMP
        if (_shieldText != null) _shieldText.text = ((int)_currentShield).ToString() + " / " + MaxShield;
        if (_hpText != null) _hpText.text = ((int)_currentHp).ToString() + " / " + MaxHP;
        //if (_shieldSlider != null) _shieldSlider.value = _currentShield / MaxShield;
        //if (_hpSlider != null) _hpSlider.value = _currentHp / MaxHP;
    }

    public void GainShield(float amount)
    {

    }
    public void GainHealth(float amount)
    {
        if (!HasStateAuthority) return;
        _currentHp += amount;
        if(_currentHp > MaxHP) _currentHp = MaxHP;

        //TEMP
        if (_shieldText != null) _shieldText.text = ((int)_currentShield).ToString() + " / " + MaxShield;
        if (_hpText != null) _hpText.text = ((int)_currentHp).ToString() + " / " + MaxHP;
        //if (_shieldSlider != null) _shieldSlider.value = _currentShield / MaxShield;
        //if (_hpSlider != null) _hpSlider.value = _currentHp / MaxHP;

    }

    void OverflowDamage(float damage)
    {
        _currentHp -= damage;
        if(_currentHp <= 0 && !_dead)
        {
            _currentHp = 0;
            RPC_Die();
        } 
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Die()
    {
        //if (!HasStateAuthority) return;
        Debug.Log("I DIED!");
        _dead = true;
        _playerState.TransitionState(PlayerFSM.GeneralState.Dead);
        GetComponent<PlayerCombatHandler>().OnDeathDropWeapons();
        //OnDeathScripts(false);
        _anim.SetTrigger("Die");
        Invoke("RPC_Revive", 3.0f);
    }

    //TEST ONLY 
    public void ResetHealth()
    {
        _dead = false;
        _playerState.TransitionState(PlayerFSM.GeneralState.Alive);
        _currentHp = MaxHP;
        _currentShield = _startingShield;
        //OnDeathScripts(true);
        //Temp
        if (_shieldText != null) _shieldText.text = ((int)_currentShield).ToString() + " / " + MaxShield;
        if (_hpText != null) _hpText.text = ((int)_currentHp).ToString() + " / " + MaxHP;
        //if (_shieldSlider != null) _shieldSlider.value = _currentShield / MaxShield;
        //if (_hpSlider != null) _hpSlider.value = _currentHp / MaxHP;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Revive()
    {
        _anim.SetTrigger("Revive");
    }

    private void OnDeathScripts(bool activate)
    {
        foreach (var script in _scriptsToDisable)
        {
            if(activate) script.enabled = true;
            else script.enabled = false;
        }
    }

    public IEnumerator DealDamageEnum(Damage damage, Vector3 forceDirection)
    {
        float damagePerTick = damage._amount / damage._tickAmount;
        float delayBetweenTicks = damage._damageOverTimeDuration / damage._tickAmount;

        yield return null;
    }



}

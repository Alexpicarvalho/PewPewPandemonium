using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityRandom = UnityEngine.Random;
using UnityEngine.UI;


public class General_Stats : MonoBehaviour, IHitable
{
    //General Properties
    [Header("General Properties")]
    [SerializeField] float _maxHP;
    [SerializeField] float _maxShield; // Changeable by armor
    [SerializeField] bool _isPushable;

    [Header("Start Properties")]
    [SerializeField] float _startingShield;
    //[SerializeField] float _startingHealth;

    //ReadOnly Variables
    public float MaxHP => _maxHP;
    public float CurrentHp => _currentHp;
    public float MaxShield => _maxShield;
    public float CurrentShield => _currentShield;
    public bool IsPushable => _isPushable;

    [Header("Runtime Properties")]
    float _currentShield;
    float _currentHp;

    [Header("Test Visuals")]
    [SerializeField] TextMeshProUGUI _hpText;
    [SerializeField] TextMeshProUGUI _shieldText;
    [SerializeField] Slider _hpSlider;
    [SerializeField] Slider _shieldSlider;

    private void Start()
    {
        _currentHp = MaxHP;
        _currentShield = _startingShield;
        
        //Temp
        if (_shieldText != null) _shieldText.text = _currentShield.ToString() + " / " + MaxShield;
        if (_hpText != null) _hpText.text = _currentHp.ToString() + " / " + MaxHP;
        if (_shieldSlider != null) _shieldSlider.value = _currentShield / MaxShield;
        if (_hpSlider != null) _hpSlider.value = _currentHp/ MaxHP;
        

       
        
    }

    public void HandleHit(Damage damage)
    {
        if (damage == null) return;
        if (_currentShield < damage._amount) OverflowDamage(Mathf.Abs(_currentShield - damage._amount));

        _currentShield -= damage._amount;
        if(_currentShield <= 0) _currentShield = 0;

        //TEMP
        if(_shieldText != null) _shieldText.text = _currentShield.ToString() + " / " + MaxShield;
        if(_hpText != null) _hpText.text = _currentHp.ToString() + " / " + MaxHP;
        if (_shieldSlider != null) _shieldSlider.value = _currentShield / MaxShield;
        if (_hpSlider != null) _hpSlider.value = _currentHp / MaxHP;
    }

    public void GainShield(float amount)
    {

    }
    public void GainHealth(float amount)
    {

    }

    void OverflowDamage(float damage)
    {
        _currentHp -= damage;
        if(_currentHp <= 0)
        {
            _currentHp = 0;
            Invoke("ResetHealth", 3.0f);
        } 
    }

    //TEST ONLY 
    public void ResetHealth()
    {
        _currentHp = MaxHP;
        _currentShield = _startingShield;
        //Temp
        if (_shieldText != null) _shieldText.text = _currentShield.ToString() + " / " + MaxShield;
        if (_hpText != null) _hpText.text = _currentHp.ToString() + " / " + MaxHP;
        if (_shieldSlider != null) _shieldSlider.value = _currentShield / MaxShield;
        if (_hpSlider != null) _hpSlider.value = _currentHp / MaxHP;
    }

    public IEnumerator DealDamageEnum(Damage damage, Vector3 forceDirection)
    {
        float damagePerTick = damage._amount / damage._tickAmount;
        float delayBetweenTicks = damage._damageOverTimeDuration / damage._tickAmount;

        yield return null;
    }



}

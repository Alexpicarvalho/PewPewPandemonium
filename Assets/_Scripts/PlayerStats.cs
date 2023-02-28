using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityRandom = UnityEngine.Random;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour, IHitable
{
    //General Properties
    [Header("General Properties")]
    [SerializeField] float _maxHP;
    [SerializeField] float _maxShield;
    [SerializeField] bool _isPushable;
    float _currentHp;
    public Slider healthBar;
    public Slider shieldBar;

    //ReadOnly Variables
    public float MaxHP => _maxHP;
    public float CurrentHp => _currentHp;
    public float _shield;

    //Temporary Shit
    public TextMeshProUGUI hpText;

    private void Start()
    {
        _currentHp = MaxHP;
        _shield = 25;
        _maxShield = 60;

    }


    private void Update()
    {
        shieldBar.value = _shield;
        healthBar.value = _currentHp;
        hpText.text = _currentHp.ToString() + " / " + MaxHP.ToString();
    }



    public void HandleHit(Damage damage)
    {
        if (_shield > 0)
        {
            _shield -= damage._amount;
        }
        else
        {
            _currentHp -= damage._amount;
        }
      //  _currentHp -= damage._amount;
        if (_currentHp <= 0)
        {
            _currentHp = 0;
            Invoke("ResetHealth", 1.5f);
        }
        
        
        
    }

    //TEST ONLY 
    public void ResetHealth()
    {
        _currentHp = MaxHP;
    }

    public IEnumerator DealDamageEnum(Damage damage, Vector3 forceDirection)
    {
        float damagePerTick = damage._amount / damage._tickAmount;
        float delayBetweenTicks = damage._damageOverTimeDuration / damage._tickAmount;

        yield return null;
    }



}

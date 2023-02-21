using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityRandom = UnityEngine.Random;


public class Stats : MonoBehaviour, IHitable
{
    //General Properties
    [Header("General Properties")]
    [SerializeField] float _maxHP;
    [SerializeField] bool _isPushable;
    float _currentHp;

    //ReadOnly Variables
    public float MaxHP => _maxHP;
    public float CurrentHp => _currentHp;

    //Temporary Shit
    public TextMeshProUGUI hpText;

    private void Start()
    {
        _currentHp = MaxHP;
    }


    private void Update()
    {
        hpText.text = _currentHp.ToString() + " / " + MaxHP.ToString();
    }



    public void HandleHit(Damage damage)
    {
        _currentHp -= damage._amount;
        if (_currentHp <= 0) _currentHp = 0;
        
        
    }

    public IEnumerator DealDamageEnum(Damage damage, Vector3 forceDirection)
    {
        float damagePerTick = damage._amount / damage._tickAmount;
        float delayBetweenTicks = damage._damageOverTimeDuration / damage._tickAmount;

        yield return null;
    }



}

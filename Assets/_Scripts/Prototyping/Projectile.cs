using CustomClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITime))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : Damager
{
    [Header("Visuals")]
    [SerializeField] GameObject _impact;

    [Header("Individual Properties")]
    public float _speed;
    public float _lifeTime = 10;

    [Header("Damage Decay")]
    [SerializeField] [Range(0, 1)] private float _minDamagePercent = 0.2f;
    [SerializeField] private float _decaysAfterSeconds;
    [SerializeField] [Range(0, 1)] private float _decayRateSecond;


    [Header("Runtime Properties")]
    private ITime iTime;
    private float _timeSinceBirth = 0;
    [Range(0, 1)] private float _currentDamagePercent = 1;
    private float _startDamage = 0;


    int collisionIndex = 0;

    // Start is called before the first frame update    
    void Start()
    {
        Destroy(gameObject, _lifeTime);
        iTime = GetComponent<ITime>();

    }

    public override void SetDamage(Damage newDamage = null)
    {
        base.SetDamage(newDamage);
        _startDamage = _damage._amount;
        Debug.Log("SET DAMAGE: " + _damage._amount);
    }
    private void Update()
    {
        transform.position += iTime.personalTimeScale * _speed * Time.deltaTime * transform.forward;
        _timeSinceBirth += Time.deltaTime;
        DecayDamage();

    }

    private void DecayDamage()
    {
        //Return Conditions
        if (_damage == null) { Debug.LogWarning("No Damage in Damager Projectile"); return; }
        if (_timeSinceBirth < _decaysAfterSeconds || _currentDamagePercent <= _minDamagePercent) return;

        _currentDamagePercent -= _decayRateSecond * Time.deltaTime;
        if (_currentDamagePercent < _minDamagePercent) _currentDamagePercent = _minDamagePercent;
        _damage._amount = _startDamage * _currentDamagePercent;
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint cp = collision.GetContact(0);
        var target = collision.collider.GetComponent<IHitable>();
        if (target != null)
        {
            target.HandleHit(_damage);
            Debug.Log(string.Format("Current Damage Percent : {0} , which result in {1} damage!", _currentDamagePercent, _damage._amount));
        }
        if (_impact != null)
        {
            var hit = Instantiate(_impact, cp.point + cp.normal * .5f, Quaternion.LookRotation(cp.normal));
            Destroy(hit, 2.0f);
        }

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IHitable>();
        if (target != null)
        {
            target.HandleHit(_damage);
        }

        if (!other.GetComponent<Collider>().isTrigger) Destroy(transform.GetComponent<Collider>());
        //Destroy(gameObject);
    }
}

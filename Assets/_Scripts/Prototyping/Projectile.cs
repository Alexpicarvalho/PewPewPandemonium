using CustomClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEditor;

[RequireComponent(typeof(PersonalTime))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : Damager
{
    [Header("Visuals")]
    [SerializeField] GameObject _impact;
    [SerializeField] Transform[] _unparentOnDestroy;

    [Header("Individual Properties")]
    public float _speed;
    public float _lifeTime = 10;
    public float _shrinkOnDestroyDuration = .1f;

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
    public virtual void Start()
    {
        Invoke("DestroyAfter", _lifeTime - _shrinkOnDestroyDuration);
        iTime = GetComponent<ITime>();
        //ApplyNetworkComponents();
    }

    //private void ApplyNetworkComponents()
    //{
    //    if (!_impact) return;

    //    if (!_impact.TryGetComponent(out NetworkObject _))
    //    {
    //        _impact.AddComponent<NetworkObject>();

    //        //Save In Disk
    //        PrefabUtility.SavePrefabAsset(_impact);
    //        AssetDatabase.SaveAssets();
    //    } 

        
    //}

    public override void SetDamage(Damage newDamage = null)
    {
        base.SetDamage(newDamage);
        _startDamage = _damage._amount;
        Debug.Log("SET DAMAGE: " + _damage._amount);
    }
    public override void FixedUpdateNetwork()
    {
        //TENS QUE MUDAR OS FILHOS DISTO NABO

        transform.position += iTime.personalTimeScale * _speed * Runner.DeltaTime * transform.forward;
        _timeSinceBirth += Runner.DeltaTime;
        DecayDamage();

    }

    private void DecayDamage()
    {
        //Return Conditions
        if (_damage == null) { Debug.LogWarning("No Damage in Damager Projectile"); return; }
        if (_timeSinceBirth < _decaysAfterSeconds || _currentDamagePercent <= _minDamagePercent) return;

        _currentDamagePercent -= _decayRateSecond * Runner.DeltaTime;
        if (_currentDamagePercent < _minDamagePercent) _currentDamagePercent = _minDamagePercent;
        _damage._amount = _startDamage * _currentDamagePercent;
        

    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        ContactPoint cp = collision.GetContact(0);
        var target = collision.collider.GetComponent<IHitable>();
        if (target != null)
        {
            target.HandleHit(_damage);
            Debug.Log(string.Format("Current Damage Percent : {0} , which result in {1} damage!", _currentDamagePercent, _damage._amount));
        }
        if (_impact != null)
        {
            var hit = Runner.Spawn(_impact, cp.point + cp.normal * .5f, Quaternion.LookRotation(cp.normal));
            Destroy(hit, 2.0f);
        }
        Destroy(gameObject);
    }

    private void UnparentTrails()
    {
        foreach (var trail in _unparentOnDestroy)
        {
            trail.parent = null;
        }
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IHitable>();
        if (target != null)
        {
            target.HandleHit(_damage);
        }

        if (!other.GetComponent<Collider>().isTrigger) Destroy(transform.GetComponent<Collider>());
        //Destroy(gameObject);
    }

    private void DestroyAfter()
    {
        StartCoroutine(DestroyMe());
    }

    private IEnumerator DestroyMe()
    {
        Vector3 startScale = transform.localScale;
        float startTime = Time.time;
        while (Time.time < startTime + _shrinkOnDestroyDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero,(Time.time - startTime)/_shrinkOnDestroyDuration);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        UnparentTrails();
    }

}

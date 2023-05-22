using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Stats_Pickup : Pickup
{

    [Header("Stat Type")]
    [SerializeField] StatDropType _dropType;
    [SerializeField] MinMaxCurve _healthReturn;
    [SerializeField] MinMaxCurve _shieldReturn;

    [Header("Visual")]
    [SerializeField] GameObject _shieldedEffect;
    [SerializeField] GameObject _healedEffect;

    public enum StatDropType { HealthCell, ShieldCell }
    // Start is called before the first frame update
    public override void Start()
    {
        _canPickUp = true;
        transform.GetComponent<SphereCollider>().isTrigger = true;
        transform.GetComponent<SphereCollider>().enabled = false;
        // StartCoroutine(CanPickUp());
        _center = transform.GetChild(0);
        _groundChecker = _center.GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _groundChecker.enabled = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<SphereCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        var playerStats = other.GetComponent<General_Stats>();

        if (playerStats != null)
        {
            switch (_dropType)
            {
                case StatDropType.HealthCell:

                    if (playerStats.CurrentHp >= playerStats.MaxHP) return;
                    else
                    {
                        playerStats.GainHealth((float)_healthReturn.Evaluate(0, Random.value));
                        var effect = Runner.Spawn(_healedEffect, other.transform.position, Quaternion.identity);
                        effect.GetComponent<FollowTarget>()._followTarget = other.transform;
                        if (HasStateAuthority) Runner.Despawn(Object);
                    }

                    break;


                case StatDropType.ShieldCell:

                    if (playerStats.CurrentShield >= playerStats.MaxShield) return;
                    else
                    {
                        playerStats.GainShield((float)_shieldReturn.Evaluate(0, Random.value));
                        var effect = Runner.Spawn(_shieldedEffect, other.transform.position, Quaternion.identity);
                        effect.GetComponent<FollowTarget>()._followTarget = other.transform;
                        if (HasStateAuthority) Runner.Despawn(Object);
                        
                    }

                    break;
            }

        }
    }
}

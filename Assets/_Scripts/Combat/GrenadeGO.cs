using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeGO : MonoBehaviour
{

    [SerializeField] public float _damage;
    [SerializeField] public float _explosionRadius;
    [SerializeField] GameObject _explosionVFX;
    private Damage damage;

    private void Start()
    {
        damage = new Damage(_damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,_explosionRadius);

        foreach (var collider in colliders)
        {
            var target = collider.GetComponent<IHitable>();

            if(target != null)
            {
                target.HandleHit(damage);
            }
        }
        Instantiate(_explosionVFX,transform.position,Quaternion.identity);
    }
}

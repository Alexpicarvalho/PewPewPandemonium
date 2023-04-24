using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlexCarvalho_Utils;
using Fusion;


public class ExplosiveGrenade : Grenade
{

    [Networked] [field: SerializeField] float damage { get; set; }
    [SerializeField] Vector3 _explosionOffset;
    [SerializeField] float _explosionRadius;
    [SerializeField] LayerMask _blocksExplosion;
    public override void Launch()
    {
        base.Launch();
    }

    public override void CalculateThrowVelocity(Vector3 target, float offset)
    {
        base.CalculateThrowVelocity(target, offset);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(name + " hit " + collision.collider);
        Explode();
        Runner.Spawn(_spawnEffect, My_Utils.SnapToGroundGetPosition(transform.position) + _explosionOffset, Quaternion.identity);
        Runner.Despawn(GetComponent<NetworkObject>());
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var collider in colliders)
        {
            var target = collider.GetComponent<IHitable>();

            if (target != null )
            {
                target.HandleHit(new Damage(damage));
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (_iTime == null) return;
        if (!_slowed && _iTime.personalTimeScale != 1)
        {
            _rb.velocity = _rb.velocity * _iTime.personalTimeScale;
            _slowed = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _explosionRadius);
    }

}

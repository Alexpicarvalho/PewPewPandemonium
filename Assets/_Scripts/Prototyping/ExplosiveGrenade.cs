using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosiveGrenade : Grenade
{
    [SerializeField] Vector3 _explosionOffset;
    [SerializeField] float _explosionRadius;
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
        Runner.Spawn(_spawnEffect, transform.position + _explosionOffset, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var collider in colliders)
        {
            var target = collider.GetComponent<IHitable>();

            if (target != null)
            {
                target.HandleHit(_damage);
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

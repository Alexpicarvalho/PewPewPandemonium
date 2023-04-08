using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GasCannisterScript : Grenade
{
    [SerializeField] GameObject _explosionVFX;
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
        if (!(_canCollideLayers == (_canCollideLayers | (1 << collision.collider.gameObject.layer)))) return;
        else
        {
            if (_spawnEffect == null) Debug.Log("SpawnEffect null");
            if (_spawnEffect == null) Debug.Log("SpawnEffect null");
            if (!Runner) Debug.Log("Runner null");
            Explode();
            Vector3 spawnPos = new Vector3(transform.position.x, 0.01f, transform.position.z);
            Runner.Spawn(_spawnEffect, spawnPos, Quaternion.identity);    
            Runner.Spawn(_explosionVFX, spawnPos + _explosionOffset, Quaternion.identity);
            Destroy(gameObject);
        }
        
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

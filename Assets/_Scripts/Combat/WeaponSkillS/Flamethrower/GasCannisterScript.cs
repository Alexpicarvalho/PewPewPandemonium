using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Explode();
            Instantiate(_spawnEffect, transform.position, Quaternion.identity);
            Instantiate(_explosionVFX, transform.position + _explosionOffset, Quaternion.identity);
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

    private void LateUpdate()
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

using CustomClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public GameObject explosionVFX;
    public float _damage = 50;
    public float _explosionRadius = 3;
    public Vector3 throwDirection;
    public Vector3 torque;
    public Vector3 explosionOffset;
    public float throwForce;
    private Rigidbody rb;
    private ITime iTime;
    private bool slowed = false;
    private Damage damage;
    [HideInInspector] public QuickGrenadeThrow throwScript;

    private void Start()
    {
        damage = new Damage(_damage);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(throwDirection * throwForce);
        rb.AddTorque(torque);
        iTime = GetComponent<ITime>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
        Instantiate(explosionVFX, transform.position + explosionOffset, Quaternion.identity);
        throwScript.IndicatorGoHome();
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
                target.HandleHit(damage);
            }
        }
    }

        private void LateUpdate()
    {
        if (iTime == null) return;
        if (!slowed && iTime.personalTimeScale != 1)
        {
            rb.velocity = rb.velocity * iTime.personalTimeScale;
            slowed = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _explosionRadius);
    }
}

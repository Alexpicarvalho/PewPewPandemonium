using CustomClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(PersonalTime))]
[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(Fusion.NetworkTransform))]
//[RequireComponent(typeof(Fusion.NetworkObject))]
public class Grenade : Damager
{
    [SerializeField] Vector3 _torque = new Vector3(45, 0, 45);
    [SerializeField] protected LayerMask _canCollideLayers;
    private Vector3 _throwDirection;
    protected Rigidbody _rb;
    protected GameObject _spawnEffect;
    public ITime _iTime;
    protected bool _slowed;

    public virtual void Launch()
    {
        GetComponent<Collider>().enabled = false;
        Invoke("ActivateCollider", .3f);
        _rb = GetComponent<Rigidbody>();
        _rb.AddTorque(_torque);
        _rb.velocity = _throwDirection /** _iTime.personalTimeScale*/;
        
    }

    public void SetSpawnEffect(GameObject effect)
    {
        _spawnEffect = effect;
    }

    public void ActivateCollider()
    {
        transform.GetComponent<Collider>().enabled = true;
    }

    public override void SetDamage(Damage newDamage = null)
    {
        base.SetDamage(newDamage);
    }

    public virtual void CalculateThrowVelocity(Vector3 target, float offset)
    {
        float displacementY = target.y - transform.position.y;
        Vector3 displacementXZ = new Vector3(target.x - transform.position.x,0, target.z - transform.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * -Physics.gravity.magnitude * offset);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * offset / -Physics.gravity.magnitude)
            + Mathf.Sqrt(2 * (displacementY - offset) / -Physics.gravity.magnitude));

        _throwDirection =  velocityXZ + velocityY;
    }



    //public void Throw(float _initialVelocity, float _angle)
    //{
    //    float angle = _angle * Mathf.Deg2Rad;
    //    StopAllCoroutines();
    //    StartCoroutine(GrenadeMovement(_initialVelocity,_angle));
    //}

    //IEnumerator GrenadeMovement(float initVel, float angle)
    //{
    //    float t = 0;
    //    while (t < 100)
    //    {
    //        float x = initVel * t * Mathf.Cos(angle);
    //        float y = initVel * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
    //        transform.position += new Vector3(x, y, 0);
    //        t += Time.deltaTime;
    //        yield return null;
    //    }
    //}


    //public GameObject explosionVFX;
    //public float _damage = 50;
    //public float _explosionRadius = 3;
    //public Vector3 throwDirection;
    //public Vector3 torque;
    //public Vector3 explosionOffset;
    //public float throwForce;
    //private Rigidbody rb;
    //private ITime iTime;
    //private bool slowed = false;
    //private Damage damage;
    //[HideInInspector] public QuickGrenadeThrow throwScript;

    //private void Start()
    //{
    //    damage = new Damage(_damage);
    //    transform.GetComponent<Collider>().enabled = false;
    //    rb = GetComponent<Rigidbody>();
    //    rb.AddForce(throwDirection * throwForce);
    //    rb.AddTorque(torque);
    //    iTime = GetComponent<ITime>();
    //    Invoke("ActivateCollider", 0.6f);
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(name + " hit " + collision.collider);
    //    Explode();
    //    Instantiate(explosionVFX, transform.position + explosionOffset, Quaternion.identity);
    //    throwScript.IndicatorGoHome();
    //    Destroy(gameObject);
    //}

    //private void ActivateCollider()
    //{
    //    transform.GetComponent<Collider>().enabled = true;
    //}

    //private void Explode()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

    //    foreach (var collider in colliders)
    //    {
    //        var target = collider.GetComponent<IHitable>();

    //        if (target != null)
    //        {
    //            target.HandleHit(damage);
    //        }
    //    }
    //}

    //    private void LateUpdate()
    //{
    //    if (iTime == null) return;
    //    if (!slowed && iTime.personalTimeScale != 1)
    //    {
    //        rb.velocity = rb.velocity * iTime.personalTimeScale;
    //        slowed = true;
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, _explosionRadius);
    //}
}

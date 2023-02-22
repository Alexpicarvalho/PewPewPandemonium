using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomClasses;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour
{
    [SerializeField] GameObject _impact;
    public float speed;
    private Rigidbody rb;
    private ITime iTime;
    public float lifeTime = 10;
    private Damage damage;
    // Start is called before the first frame update    
    void Start()
    {
        Destroy(gameObject,lifeTime);
        rb = GetComponent<Rigidbody>();
        iTime = GetComponent<ITime>();
        rb.velocity = transform.forward * speed * iTime.personalTimeScale;
        damage = new Damage(1);
    }
    private void LateUpdate()
    {
        rb.velocity = transform.forward * speed * iTime.personalTimeScale;

    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint cp = collision.GetContact(0);
        var target = collision.collider.GetComponent<IHitable>();
        if (target != null)
        {
            target.HandleHit(damage);
        }
        var hit = Instantiate(_impact, cp.point, Quaternion.LookRotation(cp.normal));
        Destroy(hit, 2.0f);
        Destroy(gameObject);
    }


}

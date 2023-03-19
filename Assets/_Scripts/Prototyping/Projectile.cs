using CustomClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITime))]
public class Projectile : Damager
{
    [SerializeField] GameObject _impact;
    public float speed;
    private ITime iTime;
    public float lifeTime = 10;

    int collisionIndex = 0;

    // Start is called before the first frame update    
    void Start()
    {
        Destroy(gameObject, lifeTime);
        iTime = GetComponent<ITime>();

    }

    public override void SetDamage(Damage newDamage = null)
    {
        base.SetDamage(newDamage);
        Debug.Log("SET DAMAGE");
    }
    private void Update()
    {
        transform.position += iTime.personalTimeScale * speed * Time.deltaTime * transform.forward;

    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint cp = collision.GetContact(0);
        var target = collision.collider.GetComponent<IHitable>();
        if (target != null)
        {
            target.HandleHit(_damage);
        }
        if (_impact != null)
        {
            var hit = Instantiate(_impact, cp.point + cp.normal * .5f, Quaternion.LookRotation(cp.normal));
            Destroy(hit, 2.0f);
        }

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IHitable>();
        if (target != null)
        {
            target.HandleHit(_damage);
        }

        if (!other.GetComponent<Collider>().isTrigger) Destroy(transform.GetComponent<Collider>());
        //Destroy(gameObject);
    }
}

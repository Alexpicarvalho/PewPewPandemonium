using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomClasses;

public class SimpleBullet : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private ITime iTime;
    public float lifeTime = 10;
    // Start is called before the first frame update    
    void Start()
    {
        Destroy(gameObject,lifeTime);
        rb = GetComponent<Rigidbody>();
        iTime = GetComponent<ITime>();
        rb.velocity = transform.forward * speed * iTime.personalTimeScale;
    }
    private void LateUpdate()
    {
        rb.velocity = transform.forward * speed * iTime.personalTimeScale;
    }

}

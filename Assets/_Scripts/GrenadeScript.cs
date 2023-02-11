using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public GameObject explosionVFX;
    public Vector3 throwDirection;
    public Vector3 torque;
    public Vector3 explosionOffset;
    public float throwForce;
    private Rigidbody rb;
    [HideInInspector]public QuickGrenadeThrow throwScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(throwDirection * throwForce);
        rb.AddTorque(torque);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionVFX, transform.position + explosionOffset, Quaternion.identity);
        throwScript.IndicatorGoHome();
        Destroy(gameObject);
    }

    //private void Update()
    //{
    //    Vector3 direction = (targetPos - transform.position).normalized;
    //    rb.AddForce(direction * throwForce, ForceMode.VelocityChange);
    //}
}

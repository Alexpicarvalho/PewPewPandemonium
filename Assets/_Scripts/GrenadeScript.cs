using CustomClasses;
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
    private ITime iTime;
    private bool slowed = false;
    [HideInInspector] public QuickGrenadeThrow throwScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(throwDirection * throwForce);
        rb.AddTorque(torque);
        iTime = GetComponent<ITime>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionVFX, transform.position + explosionOffset, Quaternion.identity);
        throwScript.IndicatorGoHome();
        Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (!slowed && iTime.personalTimeScale != 1)
        {
            rb.velocity = rb.velocity * iTime.personalTimeScale;
            slowed = true;
        }
    }
}

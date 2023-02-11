using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickGrenadeThrow : MonoBehaviour
{
    public GameObject grenade;
    public GameObject indicator;
    public float grenadeThrowForce;
    public Vector3 grenadeHeightOffset;
    public float grenadeExplosionRadius;

    [Header("Grenade Curve Calculations")]
    public float maxDistance;
    public float minDistance;
    public float minForce;
    public float maxForce;


    Vector3 indicatorHouse = new Vector3(0, 10000, 0);
    // Start is called before the first frame update
    void Start()
    {
        indicator.transform.localScale *= grenadeExplosionRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            indicator.transform.position = MousePosition();
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            LaunchGrenade();
            //indicator.transform.position = indicatorHouse;
        }
    }

    private void LaunchGrenade()
    {
       var nade = Instantiate(grenade, transform.position +
            /*indicator.transform.position +*/ grenadeHeightOffset, Quaternion.LookRotation(indicator.transform.position));
        
        nade.GetComponent<GrenadeScript>().throwDirection = CalculateThrowDirection();
        nade.GetComponent<GrenadeScript>().throwForce = grenadeThrowForce;
        nade.GetComponent<GrenadeScript>().throwScript = this;

    }

    public void IndicatorGoHome()
    {
        indicator.transform.position = indicatorHouse;
    }

    private Vector3 MousePosition()
    {
        Camera camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Plane plane = new Plane(Vector3.up, transform.position);

        if (Physics.Raycast(ray, out hit))
        {
            return new Vector3(hit.point.x , 0.1f, hit.point.z);
        }
        else return Vector3.zero;
    }

    private Vector3 CalculateThrowDirection()
    {
        Vector3 toHit = indicator.transform.position - transform.position;
        float distance = toHit.magnitude;
        distance = Math.Clamp(distance, minDistance, maxDistance);
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);

        Vector3 throwDirection;
        if (distance >= maxDistance)
        {
            throwDirection = (transform.forward + transform.up).normalized;
        }
        else if (distance <= minDistance)
        {
            throwDirection = transform.up;
        }
        else
        {
            throwDirection = Vector3.Lerp(transform.up, (transform.forward + transform.up).normalized, t);
        }
        return throwDirection;
    }


}

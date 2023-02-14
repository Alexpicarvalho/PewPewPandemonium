using CustomClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour
{
    [SerializeField] float slowDownPercentage;
        
    private void Start()
    {
        Destroy(gameObject,5.0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        ITime iTime = other.GetComponent<ITime>();

        if (iTime != null)
        {
            Debug.Log("Gotcha");
            iTime.personalTimeScale = iTime.personalTimeScale - slowDownPercentage*iTime.personalTimeScale;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ITime iTime = other.GetComponent<ITime>();

        if (iTime != null)
        {
            iTime.personalTimeScale = 1f;
        }
    }
    private void OnDestroy()
    {
        Destroy(GetComponent<Collider>());
        Collider[] colliders = Physics.OverlapSphere(transform.position, 12.0f);
        foreach (var item in colliders)
        {
            var iTime = item.GetComponent<ITime>();
            if (iTime != null)
            {
                iTime.personalTimeScale = 1;
            }
        }
        
    }
}

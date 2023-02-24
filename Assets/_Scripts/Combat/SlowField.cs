using CustomClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SlowField : MonoBehaviour
{
    [SerializeField] float slowDownPercentage;
    Animator anim;
    private List<ITime> slowedEnteties = new List<ITime> ();
        
    private void Start()
    {
        anim = GetComponent<Animator>();
        Destroy(gameObject,5.0f);
        Invoke("PlayEndAnimation",4.0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        ITime iTime = other.GetComponent<ITime>();

        if (iTime != null)
        {
            Debug.Log("Gotcha");
            slowedEnteties.Add(iTime);
            iTime.personalTimeScale = iTime.personalTimeScale - slowDownPercentage*iTime.personalTimeScale;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + "left the chat");
        ITime iTime = other.GetComponent<ITime>();
        if (iTime != null) ResetSpeeds(iTime);

    }

    private void PlayEndAnimation()
    {
        anim.Play("slowFieldDie");
    }

    private void ResetSpeeds(ITime entety)
    {   
        entety.personalTimeScale = 1;
        Debug.Log("Reset" + entety);
        slowedEnteties.Remove(entety);
    }
    private void OnDestroy()
    {
        if (slowedEnteties.Count == 0) return;
        foreach (var entety in slowedEnteties)
        {
            ResetSpeeds(entety);
        }
    }
}

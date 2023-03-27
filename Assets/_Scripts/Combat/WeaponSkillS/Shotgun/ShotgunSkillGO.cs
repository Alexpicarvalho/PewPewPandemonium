using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunSkillGO : Projectile
{
    [SerializeField] float _pullForce;
    [SerializeField] float _minDistanceOffset;
    private Vector3 _startPos;

    public override void Start()
    {
        base.Start();
        _startPos = transform.position;

    }
    public override void OnCollisionEnter(Collision collision)
    {
        var hitTarget = collision.collider.GetComponent<IHitable>();
        if (hitTarget == null) 
        {
            Debug.Log("Hit : " + collision.collider.name);
            Destroy(gameObject); 
        }
        else PullTarget(collision.collider.gameObject);



    }

    private void PullTarget(GameObject target)
    {
        Destroy(transform.GetChild(0).gameObject);
        StartCoroutine(PullTargetMovement(target));
    }

    IEnumerator PullTargetMovement(GameObject target)
    {
        Vector3 startingPos = target.transform.position;
        Vector3 pullDirection = (_startPos - startingPos).normalized;
        while (CheckDistance(target))
        {
            target.transform.position += _pullForce * Time.deltaTime * pullDirection;
            yield return null;
        }
        Destroy(gameObject);
        
    }

    private bool CheckDistance(GameObject target)
    {
        float distance = Vector3.Distance(target.transform.position,_startPos);

        if(distance <= _minDistanceOffset) return false;
        else return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunSkillGO : Projectile
{
    [SerializeField] float _pullForce;
    [SerializeField] float _minDistanceOffset;
    private Vector3 _startPos;

    public void Start()
    {
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

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        Debug.Log("I be moving");
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
        var fsm = target.GetComponent<PlayerFSM>();
        if (fsm != null) fsm.TransitionState(PlayerFSM.GeneralState.Stunned);
        while (CheckDistance(target))
        {
            target.transform.position += _pullForce * Time.deltaTime * pullDirection;
            yield return null;
        }
        if (fsm != null) fsm.TransitionState(PlayerFSM.GeneralState.Alive);
        Destroy(gameObject);
        
    }

    private bool CheckDistance(GameObject target)
    {
        float distance = Vector3.Distance(target.transform.position,_startPos);

        if(distance <= _minDistanceOffset) return false;
        else return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShowerSpawner : Projectile
{

    [SerializeField] GameObject _arrowShower;
    public Vector3 _explodePosition;
    public Vector3 _targetPos;
    private float _lastFrameDistance = 300;


    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        Debug.Log("Distance to target is : " + Vector3.Distance(transform.position, _explodePosition));
        float distance = Vector3.Distance(transform.position, _explodePosition);
        if (distance <= .1f || distance > _lastFrameDistance)
        {
            SpawnArrowShower();
            Destroy(gameObject);
        }
        _lastFrameDistance = distance;
        
    }

    private void SpawnArrowShower()
    {
        var arrowShower = Runner.Spawn(_arrowShower, _targetPos, Quaternion.identity);
    }
}

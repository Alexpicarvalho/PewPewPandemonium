using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlexCarvalho_Utils;

public class ArrowShowerSpawner : Projectile
{

    [SerializeField] GameObject _arrowShower;
    public Vector3 _explodePosition;
    public Vector3 _targetPos;
    private float _lastFrameDistance = 300;
    [SerializeField] LayerMask _ground;


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
        var arrowShower = Runner.Spawn(_arrowShower, My_Utils.SnapToGroundGetPosition(_targetPos + Vector3.up,_ground ), Quaternion.identity);
        if (arrowShower.transform.position.y < 0) arrowShower.transform.Translate(Vector3.down * arrowShower.transform.position.y);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShowerSpawner : Projectile
{

    [SerializeField] GameObject _arrowShower;
    public Vector3 _explodePosition;
    public Vector3 _targetPos;
    public override void Update()
    {
        base.Update();

        Debug.Log("Distance to target is : " + Vector3.Distance(transform.position, _explodePosition));
        if(Vector3.Distance(transform.position, _explodePosition) <= .1f)
        {
            SpawnArrowShower();
            Destroy(gameObject);
        }
        
    }

    private void SpawnArrowShower()
    {
        var arrowShower = Instantiate(_arrowShower, _targetPos, Quaternion.identity);
    }
}

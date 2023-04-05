using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerGrenade : Grenade
{
    public override void CalculateThrowVelocity(Vector3 target, float offset)
    {
        base.CalculateThrowVelocity(target, offset);
    }

    public override void Launch()
    {
        base.Launch();
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (!(_canCollideLayers == (_canCollideLayers | (1 << collision.collider.gameObject.layer)))) return;
        Instantiate(_spawnEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

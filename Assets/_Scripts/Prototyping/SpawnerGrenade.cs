using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlexCarvalho_Utils;

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
        Runner.Spawn(_spawnEffect, My_Utils.SnapToGroundGetPosition(transform.position), Quaternion.identity);
        Destroy(gameObject);
    }
}

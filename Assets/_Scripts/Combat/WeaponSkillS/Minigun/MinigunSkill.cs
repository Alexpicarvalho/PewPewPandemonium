using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Skill/Minigun", fileName = "MinigunSkill")]
public class MinigunSkill : WeaponSkillSO
{
    [SerializeField] GameObject _groundHitEffect;
    public override void ExecuteSpell(Vector3 _target = default)
    {
        base.ExecuteSpell(_target);
        var grenadeGO = _runnerNetworkBehaviour.Runner.Spawn(_skillVFX, _firePoint.position, Quaternion.identity);
        var grenadeScript = grenadeGO.GetComponent<SpawnerGrenade>();
        grenadeScript.CalculateThrowVelocity(_target, 5.0f);
        grenadeScript.Launch();
        grenadeScript.SetSpawnEffect(_groundHitEffect);
    }
}

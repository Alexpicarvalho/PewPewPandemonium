using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Skill/Flamethrower", fileName = "FlamethrowerSkill")]
public class FlamethrowerSkill : WeaponSkillSO
{
    [SerializeField] GameObject _flameFieldToSpawn;
    public override void ExecuteSpell(Vector3 _target = default)
    {
        base.ExecuteSpell(_target);
        //var skill = Instantiate(_skillVFX, _target, Quaternion.identity);
        //skill.GetComponent<Damager>().SetDamage(_damage);
        var grenadeGO = Instantiate(_skillVFX, _firePoint.position, Quaternion.identity);
        var grenadeScript = grenadeGO.GetComponent<GasCannisterScript>();
        grenadeScript.CalculateThrowVelocity(_target, 5.0f);
        grenadeScript.Launch();
        grenadeScript.SetSpawnEffect(_flameFieldToSpawn);
        grenadeScript.SetDamage(_damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Skill/Railgun", fileName = "RailgunSkill")]
public class RailgunSkill : WeaponSkillSO
{
    GameObject _castVFXClone;
    public override void ShowSkillIndicator()
    {
        base.ShowSkillIndicator();
    }

    public override void ExecuteSpell(Vector3 target = default)
    {
        base.ExecuteSpell();
        Destroy(_castVFXClone);
        var skill = _runnerNetworkBehaviour.Runner.Spawn(_skillVFX,_firePoint.position, Quaternion.LookRotation(_firePoint.forward));
       // skill.GetComponent<Damager>().SetDamage(_damage);
    }

    public override void StartCastingVFX()
    {
        _castVFXClone = Instantiate(_castVFX,_firePoint.position,Quaternion.LookRotation(_firePoint.forward),_firePoint);
    }
}

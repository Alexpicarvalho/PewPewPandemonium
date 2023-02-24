using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Skill/Railgun", fileName = "RailgunSkill")]
public class RailgunSkill : WeaponSkillSO
{

    public override void ShowSkillIndicator()
    {
        base.ShowSkillIndicator();
    }

    public override void ExecuteSpell()
    {
        base.ExecuteSpell();
        Instantiate(_skillVFX,_firePoint.position, Quaternion.LookRotation(_firePoint.forward));
    }
}

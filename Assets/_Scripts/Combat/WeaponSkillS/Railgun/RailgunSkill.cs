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

    public override void ExecuteSpell()
    {
        base.ExecuteSpell();
        Destroy(_castVFXClone);
        Instantiate(_skillVFX,_firePoint.position, Quaternion.LookRotation(_firePoint.forward));
    }

    public override void StartCastingVFX()
    {
        _castVFXClone = Instantiate(_castVFX,_firePoint.position,Quaternion.LookRotation(_firePoint.forward)); 
        _castVFXClone.transform.parent = _firePoint;
    }
}

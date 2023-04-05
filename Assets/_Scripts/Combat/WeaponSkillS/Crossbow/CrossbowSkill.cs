using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Skill/Crossbow", fileName = "CrossbowSkill")]
public class CrossbowSkill : WeaponSkillSO
{
    [Header("Aditional")]
    [SerializeField] float _aimAtHeight = 5;
    [SerializeField] GameObject _checker;
    public override void ExecuteSpell(Vector3 _target = default)
    {
        base.ExecuteSpell(_target);

        Vector3 temp = new Vector3(_target.x, _target.y + _aimAtHeight, _target.z);
        Vector3 direction = GetProjectileTargetPosition(_target);

        var spawnerArrow = Instantiate(_skillVFX, _firePoint.position, Quaternion.LookRotation(direction));
        var spawnerScript = spawnerArrow.GetComponent<ArrowShowerSpawner>();

        spawnerScript._explodePosition = temp;
        spawnerScript._targetPos = _target;

    }

    private Vector3 GetProjectileTargetPosition(Vector3 target)
    {
        Vector3 temp = new Vector3(target.x, target.y + _aimAtHeight, target.z);
        Vector3 targetPos = temp - _firePoint.position;

        return targetPos.normalized;
    }
}

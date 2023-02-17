using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[CreateAssetMenu(menuName = "Weapons/Skill", fileName = "Skill")]
public class WeaponSkillSO : ScriptableObject
{
    public float cooldown;
}

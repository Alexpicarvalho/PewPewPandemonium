using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;


[CreateAssetMenu(menuName = "Weapons/Grenade", fileName = "GrenadeSO")]
public class GrenadeSO : ScriptableObject
{
    public float cooldown;
}

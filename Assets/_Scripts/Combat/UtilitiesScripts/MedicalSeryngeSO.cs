using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;


[CreateAssetMenu(menuName = "Weapons/Utility/MedicalSerynge", fileName = "MedicalSeryngeSO")]
public class MedicalSeryngeSO : UtilitySO
{
    [Header("Individual Properties")]
    public float _healAmount;

    [Header("References")]
    private General_Stats _playerStats;

    public override void SetupUtility(Transform player, Transform firePoint)
    {
        base.SetupUtility(player, firePoint);
        _playerStats = player.GetComponent<General_Stats>();

    }
    public override void Use()
    {
        base.Use();
        if (_onCooldown) return;

        _onCooldown = true;
        _timeSinceLastUse = 0;
        _playerStats.GainHealth(_healAmount);
    }
}

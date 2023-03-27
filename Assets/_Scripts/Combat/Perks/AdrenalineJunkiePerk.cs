using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perk/Adrenaline Junkie", fileName = "AdrenalineJunkiePerk")]
public class AdrenalineJunkiePerk : Perk
{
    [SerializeField] private float _speedPerLvl;
    public override void UpdateEffects(GameObject player)
    {
        if (_reachedMaxLevel) return;
        base.UpdateEffects(player);

        var moveScript = player.GetComponent<PlayerMovement>();
        moveScript.PerkModifySpeed(_speedPerLvl);
    }
}
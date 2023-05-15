using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUiManager : MonoBehaviour
{

    public static PlayerUiManager playerUI;
    public TempUIInfo _uiRefs;

    private void Awake()
    {
        if (playerUI != null) Destroy(gameObject);
        else playerUI = this;
    }

    public void RefreshStats(General_Stats _stats)
    {
    }

    public void RefreshWeapons(PlayerCombatHandler combatHandeler)
    {

    }

    public void RefreshAmmo(PlayerCombatHandler combatHandeler)
    {

    }

    public void RefreshSkill(PlayerCombatHandler combatHandeler)
    {

    }

    public void RefreshUtility(PlayerCombatHandler combatHandeler)
    {

    }

    public void RefreshGrenade(PlayerCombatHandler combatHandeler)
    {

    }

}

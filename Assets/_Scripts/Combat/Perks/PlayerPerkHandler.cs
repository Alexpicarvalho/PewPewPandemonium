using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPerkHandler : MonoBehaviour
{
    [SerializeField] List<Perk> _perks = new List<Perk>();
    //private List<Perk> _perks = new List<Perk>();

    [Header("Relevant Components")]
    private PlayerMovement _playerMovement;
    private General_Stats _playerStats;
    [SerializeField] private CameraControler _cameraControler;
    private PlayerCombatHandler _playerCombatHandler;

    Button button;

    private void Start()
    {
        GetComponents();
        InstatiatePerks();
    }


    //UPDATE IS TEST ONLY
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (var perk in _perks)
            {
                if(perk is AdrenalineJunkiePerk)
                {
                    perk.GetCamController(_cameraControler);
                    perk.LevelUp();
                    perk.UpdateEffects(transform.gameObject);
                }
            }
        }
    }

    private void GetComponents()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerStats = GetComponent<General_Stats>();
        _playerCombatHandler = GetComponent<PlayerCombatHandler>();
    }

    private void InstatiatePerks()
    {
        foreach (var perk in /*_perkRefs*/_perks)
        {
            perk._points = 0;
            perk._reachedMaxLevel = false;
            //Perk newPerk = Instantiate(perk);
            //_perks.Add(newPerk);
        }
    }

    private void RecalculateEffects()
    {
        foreach (var perk in _perks)
        {
            perk.UpdateEffects(transform.gameObject);
        }
    }

    public void LevelUpPerk()
    {
    }
}

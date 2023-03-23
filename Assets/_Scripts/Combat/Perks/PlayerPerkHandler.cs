using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerkHandler : MonoBehaviour
{
    [SerializeField] List<Perk> _perkRefs = new List<Perk>();
    private List<Perk> _perks = new List<Perk>();

    [Header("Relevant Player Components")]
    private PlayerMovement _playerMovement;
    private General_Stats _playerStats;


    private void Start()
    {
        GetComponents();
        InstatiatePerks();
    }

    private void GetComponents()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerStats = GetComponent<General_Stats>();
    }

    private void InstatiatePerks()
    {
        foreach (var perk in _perkRefs)
        {
            Perk newPerk = Instantiate(perk);
            _perks.Add(newPerk);
        }
    }

    private void RecalculateEffects()
    {
        foreach (var perk in _perks)
        {
            perk.UpdateEffects();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEngine.ParticleSystem;
using Fusion;

public class WeaponPickUp : Pickup
{
    [SerializeField] public GunSO _weaponToGive;
    [Networked][field :SerializeField] public WeaponTier _weaponToGiveTier { get; set; }
    [SerializeField] private float _startThrowForce = 100f;
    [SerializeField] private ParticleSystemRenderer _glowEffect;
    [SerializeField] private Material _commonGlow;
    [SerializeField] private Material _uncommonGlow;
    [SerializeField] private Material _rareGlow;
    [SerializeField] private Material _specialGlow;

    //[Networked (OnChanged = nameof(SelectGlowColor))]
    public NetworkBool _colorChanged { get; set; }

    GunSO _gun;
    Rigidbody _rb;

    public override void Start()
    {
        base.Start();

        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = false;
        Vector3 temp = (Vector3.up + RandomDirectionalVector() * (Random.Range(0, 2) * 2 - 1)) * _startThrowForce;
        _rb.AddForce(temp);
        _gun = Instantiate(_weaponToGive);
        _gun._weaponTier = _weaponToGiveTier;

        SelectGlowColor();
    }


    private void SelectGlowColor()
    {
        Debug.Log("Weapon tier : " + _weaponToGive._weaponTier);
        switch (_weaponToGiveTier)
        {
            case WeaponTier.Tier3:
                _glowEffect.material = _commonGlow;
                _rarity = Rarity.Common;
                break;
            case WeaponTier.Tier2:
                _glowEffect.material = _uncommonGlow;
                _rarity = Rarity.Rare;
                break;
            case WeaponTier.Tier1:
                _glowEffect.material = _rareGlow;
                _rarity = Rarity.Epic;
                break;
            case WeaponTier.Special:
                _glowEffect.material = _specialGlow;
                _rarity = Rarity.Legendary;
                break;
        }
    }

    private Vector3 RandomDirectionalVector()
    {
        Vector3 returnVec = Random.onUnitSphere;

        return returnVec;
    }

    public override void PickMeUp(PlayerCombatHandler _anyPlayer)
    {
        base.PickMeUp(_anyPlayer);
        _anyPlayer.ReceiveWeapon(_gun);
        Destroy(gameObject);
    }
}

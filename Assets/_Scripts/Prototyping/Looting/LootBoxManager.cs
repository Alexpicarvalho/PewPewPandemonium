using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class LootBoxManager : MonoBehaviour
{
    public static LootBoxManager Instance { get; private set; }

    [Header("Possible Loot")]
    [SerializeField] private List<GameObject> _dropGunsPref = new List<GameObject>();
    [SerializeField] private List<GameObject> _dropGrenades = new List<GameObject>();
    [SerializeField] private List<GameObject> _dropMedicine = new List<GameObject>();
    [SerializeField] private List<GameObject> _dropWalkables = new List<GameObject>();
    [SerializeField] private GameObject _dropXp;

    
    [Header("Common Percentages")]

    [Header("Drop Chances")]

    [SerializeField] private float _commonBoxT1GunDropChance;
    [SerializeField] private float _commonBoxT2GunDropChance;
    [SerializeField] private float _commonBoxT3GunDropChance;
    [SerializeField] private float _commonBoxGrenadeDropChance;
    [SerializeField] private float _commonBoxMedicineDropChance;
    [SerializeField] private float _commonBoxWalkableDropChance;
    [SerializeField] private MinMaxCurve _commonBoxXpAmount;

    [Header("Rare Percentages")]
    [SerializeField] private float _rareBoxT1GunDropChance;
    [SerializeField] private float _rareBoxT2GunDropChance;
    [SerializeField] private float _rareBoxT3GunDropChance;
    [SerializeField] private float _rareBoxGrenadeDropChance;
    [SerializeField] private float _rareBoxMedicineDropChance;
    [SerializeField] private float _rareBoxWalkableDropChance;
    [SerializeField] private MinMaxCurve _rareBoxXpAmount;

    [Header("Epic Percentages")]
    [SerializeField] private float _epicBoxT1GunDropChance;
    [SerializeField] private float _epicBoxT2GunDropChance;
    [SerializeField] private float _epicBoxT3GunDropChance;
    [SerializeField] private float _epicBoxGrenadeDropChance;
    [SerializeField] private float _epicBoxMedicineDropChance;
    [SerializeField] private float _epicBoxWalkableDropChance;
    [SerializeField] private MinMaxCurve _epicBoxXpAmount;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }


    public List<GameObject> BoxRequestDrops(Rarity _boxRarity)
    {
        List<GameObject> returnDrops = new List<GameObject>();

        // Create the instance of XP (100% chance of dropping)
        returnDrops.Add(CreateXpInstance(_boxRarity));

        // Pick the weapon that is going to drop
        GameObject weaponToDrop = _dropGunsPref[WeaponToReturnIndex()]; // Selects Weapon
        returnDrops.Add(weaponToDrop);

        return returnDrops;
    }

    public WeaponTier RequestWeaponTier(Rarity _boxRarity)
    {
        return PickDropWeaponTier(_boxRarity);
    }

    private int WeaponToReturnIndex()
    {
        return Random.Range(0, _dropGunsPref.Count);
    }
    private WeaponTier PickDropWeaponTier(Rarity _boxRarity)
    {

        switch (_boxRarity)
        {
            case Rarity.Common:
                return TryGetTier(_commonBoxT1GunDropChance, _commonBoxT2GunDropChance, _commonBoxT3GunDropChance);

            case Rarity.Rare:
                return TryGetTier(_rareBoxT1GunDropChance, _rareBoxT2GunDropChance, _rareBoxT3GunDropChance);

            case Rarity.Epic:
                return TryGetTier(_epicBoxT1GunDropChance, _epicBoxT2GunDropChance, _epicBoxT3GunDropChance);

        }
        return WeaponTier.Tier3;
    }

    private WeaponTier TryGetTier(float tier1Chance, float tier2Chance, float tier3Chance)
    {
        float check = Random.Range(0.0f, 1.0f);
        Debug.Log($"Check was {check}, T1: {tier1Chance}, T2: {tier2Chance}, T3: {tier3Chance} ");

        if (tier1Chance >= check) return WeaponTier.Tier1;
        else if (tier2Chance >= check) return WeaponTier.Tier2;
        else return WeaponTier.Tier3;


    }

    private GameObject CreateXpInstance(Rarity _boxRarity)
    {
        GameObject xpInstance = _dropXp;
        var xpInstanceScript = xpInstance.GetComponent<XP_PickUp>();
        switch (_boxRarity)
        {
            case Rarity.Common:
                xpInstanceScript._xpToGive = (int)_commonBoxXpAmount.Evaluate(1, Random.value);
                break;
            case Rarity.Rare:
                xpInstanceScript._xpToGive = (int)_rareBoxXpAmount.Evaluate(1, Random.value);
                break;
            case Rarity.Epic:
                xpInstanceScript._xpToGive = (int)_epicBoxXpAmount.Evaluate(1, Random.value);
                break;
            default:
                break;
        }

        return xpInstance;
    }
}

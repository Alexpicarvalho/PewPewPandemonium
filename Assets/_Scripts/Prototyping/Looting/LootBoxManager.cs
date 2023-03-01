using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class LootBoxManager : MonoBehaviour
{
    public static LootBoxManager Instance { get; private set; }

    [Header("Possible Loot")]
    [SerializeField] private List<GameObject> _dropGunsPref = new List<GameObject>();
    [SerializeField] private List<GameObject> _dropGrenades = new List<GameObject>();
    [SerializeField] private List<GameObject> _dropMedicine = new List<GameObject>();
    [SerializeField] private List<GameObject> _dropWalkables = new List<GameObject>();

    [Header("Drop Chances")]
    [Header("Common Percentages")]
    [SerializeField] private float _commonBoxT1GunDropChance;
    [SerializeField] private float _commonBoxT2GunDropChance;
    [SerializeField] private float _commonBoxT3GunDropChance;
    [SerializeField] private float _commonBoxGrenadeDropChance;
    [SerializeField] private float _commonBoxMedicineDropChance;
    [SerializeField] private float _commonBoxWalkableDropChance;
    [SerializeField] private MinMaxCurve _commonBoxXpAmount;

    [Header("Uncommon Percentages")]
    [SerializeField] private float _uncommonBoxT1GunDropChance;
    [SerializeField] private float _uncommonBoxT2GunDropChance;
    [SerializeField] private float _uncommonBoxT3GunDropChance;
    [SerializeField] private float _uncommonBoxGrenadeDropChance;
    [SerializeField] private float _uncommonBoxMedicineDropChance;
    [SerializeField] private float _uncommonBoxWalkableDropChance;
    [SerializeField] private MinMaxCurve _uncommonBoxXpAmount;

    [Header("Rare Percentages")]
    [SerializeField] private float _rareBoxT1GunDropChance;
    [SerializeField] private float _rareBoxT2GunDropChance;
    [SerializeField] private float _rareBoxT3GunDropChance;
    [SerializeField] private float _rareBoxGrenadeDropChance;
    [SerializeField] private float _rareBoxMedicineDropChance;
    [SerializeField] private float _rareBoxWalkableDropChance;
    [SerializeField] private MinMaxCurve _rareBoxXpAmount;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }

}

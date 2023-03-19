using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(Rigidbody))]
public class LootBox : MonoBehaviour, IHitable
{
    [Header("Box Stats")]
    [SerializeField] private float _maxHp = 10;
    [SerializeField] public float _currentHP;
    [SerializeField] public Rarity _rarity;

    [Header("Visual")]
    [SerializeField] private ParticleSystemRenderer _glowEffect;
    [SerializeField] private Material _glowCommon;
    [SerializeField] private Material _glowRare;
    [SerializeField] private Material _glowEpic;

    [Header("Explosions")]
    [SerializeField] private GameObject _commonExplosionVFX;
    [SerializeField] private GameObject _rareExplosionVFX;
    [SerializeField] private GameObject _epicExplosionVFX;
    [SerializeField] private GameObject _legendaryExplosionVFX;

    [Header("Temporary Test Values")]
    public List<GameObject> _drops = new List<GameObject>();
    public WeaponTier _weaponTier;


    private bool isQuitting;

    private void Start()
    {
        _drops = LootBoxManager.Instance.BoxRequestDrops(_rarity);
        SetGlowColor();
    }

    private void SetGlowColor()
    {
        switch (_rarity)
        {
            case Rarity.Common:
                _glowEffect.material = _glowCommon;
                break;
            case Rarity.Rare:
                _glowEffect.material = _glowRare;
                break;
            case Rarity.Epic:
                _glowEffect.material = _glowEpic;
                break;
        }
    }

    public void HandleHit(Damage damage)
    {
        if (damage == null) return;
        _currentHP -= damage._amount;
        if (_currentHP <= 0) Destroy(gameObject);
    }

    public void InstantDestroy() { Destroy(gameObject); }

    private void OnDestroy()
    {
        if (isQuitting) return;
        PlayExplosion();
        foreach (var drop in _drops)
        {
            var weaponInstance = Instantiate(drop, transform.position
                + new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y)
                + Vector3.up * 2, Quaternion.identity);

            var _weaponScript = weaponInstance.GetComponent<WeaponPickUp>();
            if (_weaponScript != null)
            {
                _weaponTier = LootBoxManager.Instance.RequestWeaponTier(_rarity);
                _weaponScript._weaponToGiveTier = _weaponTier;
                Debug.Log("Weapon Tier is " + _weaponTier); 
            }

        }
    }

    private void PlayExplosion()
    {
        switch (_rarity)
        {
            case Rarity.Common:
                Instantiate(_commonExplosionVFX, transform.position, Quaternion.identity);
                break;
            case Rarity.Rare:
                Instantiate(_rareExplosionVFX, transform.position, Quaternion.identity);
                break;
            case Rarity.Epic:
                Instantiate(_epicExplosionVFX, transform.position, Quaternion.identity);
                break;
            case Rarity.Legendary:
                Instantiate(_legendaryExplosionVFX, transform.position, Quaternion.identity);
                break;
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }


}

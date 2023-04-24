using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEngine.ParticleSystem;
using Fusion;

[RequireComponent(typeof(Rigidbody))]
public class LootBox : NetworkBehaviour, IHitable
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
    [SerializeField] private SoundData _explosionSound;

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
        if (_currentHP <= 0) 
        {
            RPC_SendLootInfoToServer();
        } 
    }

    public void InstantDestroy() 
    {
        RPC_SendLootInfoToServer();
    }


    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    private void RPC_SendLootInfoToServer()
    {
        PlayExplosion();
        foreach (var drop in _drops)
        {
            var weaponInstance = Runner.Spawn(drop, transform.position
                + new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y)
                + Vector3.up * 2, Quaternion.identity);

            if (weaponInstance == null) Debug.Log("Weapon Instance null");

            var _weaponScript = weaponInstance.GetComponent<WeaponPickUp>();
            if (_weaponScript != null)
            {
                _weaponTier = LootBoxManager.Instance.RequestWeaponTier(_rarity);
                _weaponScript._weaponToGiveTier = _weaponTier;
                Debug.Log("Weapon Tier is " + _weaponTier);
            }
            else Debug.Log("No WeaponPickUp");

        }
        Runner.Despawn(Object);
    }

    private void PlayExplosion()
    {
        AudioSource.PlayClipAtPoint(_explosionSound.GetRandomSound(), transform.position, _explosionSound.GetClipVolume());
        switch (_rarity)
        {
            case Rarity.Common:
                Runner.Spawn(_commonExplosionVFX, transform.position, Quaternion.identity);
                break;
            case Rarity.Rare:
                Runner.Spawn(_rareExplosionVFX, transform.position, Quaternion.identity);
                break;
            case Rarity.Epic:
                Runner.Spawn(_epicExplosionVFX, transform.position, Quaternion.identity);
                break;
            case Rarity.Legendary:
                Runner.Spawn(_legendaryExplosionVFX, transform.position, Quaternion.identity);
                break;
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }


}

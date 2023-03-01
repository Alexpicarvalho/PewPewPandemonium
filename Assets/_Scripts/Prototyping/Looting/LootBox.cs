using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class LootBox : MonoBehaviour, IHitable
{
    [Header("Box Stats")]
    [SerializeField] private float _maxHp = 10;
    [SerializeField] public float _currentHP;
    [SerializeField] public Rarity _rarity;

    [Header("Visual")]
    [SerializeField] private GameObject _explosionVFX;

    [Header("Temporary Test Values")]
    public List<GameObject> _drops = new List<GameObject>();


    private bool isQuitting;


    public void HandleHit(Damage damage)
    {
        _currentHP -= damage._amount;
        if(_currentHP <= 0) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (isQuitting) return;
        Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        foreach (var drop in _drops)
        {
            Instantiate(drop, transform.position 
                + new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y)
                + Vector3.up * 2, Quaternion.identity);

        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }


}

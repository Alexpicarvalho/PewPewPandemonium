using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityRandom = UnityEngine.Random;

public class PlayerLoot : MonoBehaviour
{
    [SerializeField] private float _pickUpRadius;
    [SerializeField] [Range(0, 6)] private int _maxPickupsInMenu;
    [SerializeField] LayerMask _pickupLayerMask;
    [SerializeField] private float _inputCooldown;
    private Pickup _nearestPickup;
    List<Pickup> _nearbyPickups = new List<Pickup>();
    List<Pickup> _lastFrameList = new List<Pickup>();
    List<Pickup> _finalList = new List<Pickup>();
    
    bool _anyPickup = false;
    PlayerCombatHandler _combatHandler;
    float _timeSinceLastPickupAttempt;

    [Header("Pick up game UI")]
    [SerializeField] GameObject _groundPickupsUI;
    GameObject[] _groundPickupSlots;
    GameObject _titleText;

    [Header("Loot Me overlay")]
    [SerializeField] GameObject _lootMEPrefab;
    [SerializeField] Vector3 _offset;


    private void Start()
    {
        SetupMenu();
        _combatHandler = GetComponent<PlayerCombatHandler>();

    }

    private void SetupMenu()
    {
        _groundPickupSlots = new GameObject[_maxPickupsInMenu];
        _titleText = _groundPickupsUI.transform.GetChild(6).gameObject;

        for (int i = 0; i < _groundPickupSlots.Length; i++)
        {
            _groundPickupSlots[i] = _groundPickupsUI.transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        _timeSinceLastPickupAttempt += Time.deltaTime;
        DetectPickups();
        ClearLists();
        GetNearestPickup();
        ShowInList();
        VisualiseInMenu();

        if (_timeSinceLastPickupAttempt >= _inputCooldown) DetectPlayerInput();
    }

    private void ClearLists()
    {
        _finalList = _lastFrameList.Intersect(_nearbyPickups).ToList();
    }

    private void LateUpdate()
    {
        _lastFrameList = _nearbyPickups;
        _nearbyPickups.Clear();
    }

    private void VisualiseInMenu()
    {
        for (int i = 0; i < _groundPickupSlots.Length; i++)
        {
            _groundPickupSlots[i].SetActive(false);
        }

        if (_nearbyPickups.Count == 0)
        {
            _titleText.SetActive(false);
            return;
        }
        //First activate the Header if there is at least one in list
        _titleText.SetActive(true);

        //Set all false, then turn back on the right number (might be optimizable)



        for (int i = 0; i < _finalList.Count; i++)
        {
            Transform mask = _groundPickupSlots[i].transform.GetChild(0);
            mask.GetChild(0).GetComponent<RawImage>().texture = _finalList[i]._pickupIcon;
            mask.GetComponent<Image>().color = SetMaskColor(mask, _finalList[i]);
            _groundPickupSlots[i].SetActive(true);
        }

    }

    private Color SetMaskColor(Transform mask, Pickup pickup)
    {
        switch (pickup._rarity)
        {
            case Rarity.Common:
                return Color.green;

            case Rarity.Rare:
                return Color.blue;

            case Rarity.Epic:
                return  new Color(1,0,1);

            case Rarity.Legendary:
                return new Color(1,.5f,.5f);
                default:
                return Color.white;
        }
    }

    public void PickUpOnButton(int buttonID)
    {
        if (_timeSinceLastPickupAttempt < _inputCooldown && _nearbyPickups.Count == 0) return;

        Debug.Log(buttonID);
        _finalList[buttonID].PickMeUp(_combatHandler);
    }

    private void DetectPlayerInput()
    {
        if (_nearestPickup != null && Input.GetKeyDown(KeyCode.F))
        {
            _nearestPickup.PickMeUp(_combatHandler);
            _timeSinceLastPickupAttempt = 0;
        }
    }

    private void GetNearestPickup()
    {
        if (_finalList.Count == 0)
        {
            _nearestPickup = null;
            _lootMEPrefab.transform.position = Vector3.up*1000;
            return;
        }

        float[] distances = new float[_nearbyPickups.Count];
        int index = 0;
        foreach (var pickup in _finalList)
        {
            distances[index] = Vector3.Distance(transform.position, pickup.transform.position);
            index++;
        }
        int pickupIndex = Array.IndexOf(distances, distances.Min());
        _nearestPickup = _nearbyPickups[pickupIndex];

        _lootMEPrefab.transform.position = _nearestPickup.transform.position + _offset;
        _lootMEPrefab.GetComponentInChildren<TextMeshProUGUI>().text = _nearestPickup._rarity.ToString();
    }

    private void ShowInList()
    {
        foreach (var pickup in _nearbyPickups)
        {
            //Mostrar no menu lateral o icon e nome do objeto 
        }
    }

    private void DetectPickups()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _pickUpRadius, _pickupLayerMask);

        foreach (var collider in colliders)
        {
            var pickup = collider.GetComponent<Pickup>();

            if (pickup == null || _nearbyPickups.Contains(pickup) || _nearbyPickups.Count >= _maxPickupsInMenu) continue;

            _nearbyPickups.Add(pickup);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 1, .5f);
        Gizmos.DrawSphere(transform.position, _pickUpRadius);
    }
}

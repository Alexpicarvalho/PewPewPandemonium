//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityRandom = UnityEngine.Random;

//public class PlayerLoot : MonoBehaviour
//{
//    [SerializeField] private float _pickUpRadius;
//    [SerializeField] private int _maxPickupsInMenu;
//    [SerializeField] LayerMask _pickupLayerMask;
//    private Pickup _nearestPickup;
//    List<ScriptableObject> _nearbyPickups = new List<ScriptableObject> ();
//    bool _anyPickup = false;


//    private void Update()
//    {
//        DetectPickups();
//        GetnearestPickup();
//        ShowInList();
        

//        _nearbyPickups.Clear();
//    }

//    private void GetnearestPickup()
//    {
        
//    }

//    private void ShowInList()
//    {
//        foreach (var pickup in _nearbyPickups)
//        {
//            //Mostrar no menu lateral o icon e nome do objeto 
//        }
//    }

//    private void DetectPickups()
//    {
//        Collider[] colliders = Physics.OverlapSphere(transform.position, _pickUpRadius, _pickupLayerMask);

//        foreach (var collider in colliders)
//        {
//            var pickup = collider.GetComponent<Pickup>();

//            if (pickup == null) continue;

//            _nearbyPickups.Add(pickup);
//        }

//    }
//}

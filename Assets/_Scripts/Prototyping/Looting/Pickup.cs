using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(Rigidbody))]

public class Pickup : NetworkBehaviour
{
    [SerializeField] float _newPickUpDelay = 1;
    [SerializeField] public float _pickUpRange = 2;
    public Texture2D _pickupIcon;
    public Rarity _rarity;
    [HideInInspector] public Collider _groundChecker;
    [HideInInspector] public bool _canPickUp;
    [HideInInspector] public Transform _center;
    // Start is called before the first frame update
    public virtual void Start()
    {
        _canPickUp = true;
        transform.GetComponent<BoxCollider>().isTrigger = true;
        transform.GetComponent<BoxCollider>().enabled = false;
       // StartCoroutine(CanPickUp());
        _center = transform.GetChild(0);
        _groundChecker = _center.GetComponent<Collider>();
    }


    public virtual void PickMeUp(PlayerCombatHandler _anyPlayer) { if(!_canPickUp) return; }

    IEnumerator CanPickUp()
    {
        yield return new WaitForSeconds(_newPickUpDelay);
        _canPickUp = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        _groundChecker.enabled = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<BoxCollider>().enabled = true;

    }
}

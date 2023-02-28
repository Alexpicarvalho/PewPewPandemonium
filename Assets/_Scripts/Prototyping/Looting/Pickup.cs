using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Pickup : MonoBehaviour
{
    [SerializeField] float _newPickUpDelay = 1;
    [SerializeField] public float _pickUpRange = 2;
    [HideInInspector] public Collider _groundChecker;
    [HideInInspector] public bool _canPickUp;
    [HideInInspector] public List<PlayerCombatHandler> _playersInRange = new List<PlayerCombatHandler>();
    [HideInInspector] public Transform _center;
    // Start is called before the first frame update
    public virtual void Start()
    {
        _canPickUp = false;
        transform.GetComponent<BoxCollider>().isTrigger = true;
        StartCoroutine(CanPickUp());
        _center = transform.GetChild(0);
        _groundChecker = _center.GetComponent<Collider>();
    }


    public virtual void PickMeUp() { }

    IEnumerator CanPickUp()
    {
        yield return new WaitForSeconds(_newPickUpDelay);
        Debug.Log("Can pick up");
        _canPickUp = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        _groundChecker.enabled = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;

    }
}

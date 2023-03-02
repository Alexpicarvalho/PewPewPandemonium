using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class XP_PickUp : Pickup
{
    //TEST ONLY
    private GameObject _player;

    private bool _readyToChase = false;
    [SerializeField] float _chaseDelay;
    [SerializeField] float _chaseStartSpeed;
    [SerializeField] float _chaseAccelaration;
    [HideInInspector] public int _xpToGive;

    public override void Start()
    {
        base.Start();
        _player = FindObjectOfType<PlayerCombatHandler>().gameObject; //TEST
        StartCoroutine(ReadyChase());
    }

    public void Update()
    {
        if (_readyToChase)
        {
            transform.position += (_player.transform.position - transform.position).normalized
                * _chaseStartSpeed * Time.deltaTime;

            _chaseStartSpeed += _chaseAccelaration * Time.deltaTime;
        }

        if(_canPickUp && (_player.transform.position - transform.position).magnitude <= 2)
        {
            //Give Player XP
            Destroy(gameObject);
        }
    }


    IEnumerator ReadyChase()
    {
        yield return new WaitForSeconds(_chaseDelay);
        _readyToChase = true;
    }

}

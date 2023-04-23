using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MinimapRepresentation : NetworkBehaviour
{
    [SerializeField] Transform _playerVisual;
    [SerializeField] Transform _enemyVisual;
    private void Awake()
    {
        
    }

    public override void Spawned()
    {
        base.Spawned();
        if(GetComponentInParent<NetworkObject>().HasInputAuthority)
        {
            _enemyVisual.gameObject.SetActive(false);
            _playerVisual.gameObject.SetActive(true);
        }
        else
        {
            _playerVisual.gameObject.SetActive(false);
            _enemyVisual.gameObject.SetActive(true);
        }
    }

}

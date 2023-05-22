using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class VFXHandler : NetworkBehaviour
{
    [SerializeField] float _destroyAfterTime;
    bool _isNetworkObject;

    private void Awake()
    {
        _isNetworkObject = TryGetComponent<NetworkObject>(out _);
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!_isNetworkObject) Destroy(gameObject,_destroyAfterTime);
    }

    public override void Spawned()
    {
        if (_isNetworkObject && HasStateAuthority) Invoke(nameof(DespawnObject), _destroyAfterTime);
    }

    void DespawnObject()
    {
        Runner.Despawn(Object);
    }
}

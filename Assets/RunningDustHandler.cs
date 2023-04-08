using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
public class RunningDustHandler : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private ParticleSystem _dustPS;
    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _dustPS = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (_playerMovement.Speed == _playerMovement.MaxSpeed && _dustPS.enableEmission == false) _dustPS.enableEmission = true;
        //else _dustPS.enableEmission = false;
    }
}

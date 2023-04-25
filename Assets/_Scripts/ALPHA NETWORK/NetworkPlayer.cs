using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Cinemachine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            

            //Camera myCamera = GetComponentInChildren<Camera>();
            //myCamera.enabled = true;
            

            //disable main camera
            //Camera.main.gameObject.SetActive(false);

            Debug.Log("Spawned local player");
        }
        else {
            // disable the camera if we are not the local player
            //Camera localCamera = GetComponentInChildren<Camera>();
            //localCamera.enabled = false;

            CinemachineVirtualCamera cmCam = GetComponentInChildren<CinemachineVirtualCamera>();
            cmCam.enabled = false;

            //AudioListener audioListener = GetComponentInChildren<AudioListener>();
            //audioListener.enabled = false;

            Debug.Log("Spawned remote player"); 
        }

        transform.name = $"Player_{Object.Id}";
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
            Runner.Despawn(Object);

    }
}

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

            //GetComponentInChildren<Minimap>().target = transform;

            //Camera myCamera = GetComponentInChildren<Camera>();
            //myCamera.enabled = true;


            //disable main camera
            //Camera.main.gameObject.SetActive(false);

            FindObjectOfType<CameraControler>().SetFollowTarget(transform);
            FindObjectOfType<Minimap>().target = transform;
            var UI = transform.Find("UI");
            if (UI != null)
            {
                UI.gameObject.SetActive(true);
                UI.GetComponent<TempUIInfo>()._player = transform;
            }

            Debug.Log("Spawned local player with ID -> " + Object.Id);
        }
        else
        {
            // disable the camera if we are not the local player
            //Camera localCamera = GetComponentInChildren<Camera>();
            //localCamera.enabled = false;

            //CinemachineVirtualCamera cmCam = GetComponentInChildren<CinemachineVirtualCamera>();
            //cmCam.enabled = false;

            //AudioListener audioListener = GetComponentInChildren<AudioListener>();
            //audioListener.enabled = false;

            Debug.Log("Spawned remote player");
        }

        ZoneDamage.Instance.AddPlayerInside(transform);
        GameManager.Instance.AddPlayerToList(transform);

        transform.name = $"Player_{Object.Id}";
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
            Runner.Despawn(Object);

    }
}

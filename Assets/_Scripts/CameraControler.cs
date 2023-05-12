using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using Cinemachine;
using Fusion;


public class CameraControler : NetworkBehaviour
{
    float _cameraDistance;
    [SerializeField] float _defaultCameraDistance;
    [SerializeField] float _shootingCameraDistance;
    [SerializeField] float _waitBeforeDistanceReset;
    [SerializeField] float _confirmStopShooting = .7f;
    [SerializeField] PlayerCombatHandler _combatHandler;
    private CinemachineVirtualCamera _cinCam;
    bool _shooting = false;
    private Camera _mainCam;
    private CinemachineBrain _cmBrain;
    private Transform _player;
    float _stopShootingTimer = 0;
    private void Start()
    {
        _cinCam = GetComponent<CinemachineVirtualCamera>();
        _cameraDistance = _defaultCameraDistance;
        _mainCam = Camera.main;
        _cmBrain = _mainCam.GetComponent<CinemachineBrain>();
        //_player = transform.parent;
        

    }

    public void SetFollowTarget(Transform target)
    {
        if (_cinCam.Follow != null) return;
        _cinCam.Follow = target;
    }

    private void Update()
    {
        _stopShootingTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            StopCoroutine(ResetCamera());
            _stopShootingTimer = 0;
            _shooting = true;
            _cameraDistance =_defaultCameraDistance + _shootingCameraDistance * _combatHandler._gun._weaponSights;
        }
        else
        {
            if(_stopShootingTimer >= _confirmStopShooting)
            StartCoroutine(ResetCamera());
        }

        //_cameraDistance -= Input.GetAxis("Mouse ScrollWheel");
        //_cameraDistance = Mathf.Clamp(_cameraDistance,6, 20);
        var componentBase = _cinCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer)
        {
            (componentBase as CinemachineFramingTransposer).m_CameraDistance = _cameraDistance;
        }
    }

    public void LateUpdate()
    {
        //Debug.Log("Entered Network Update cycle");
        _cmBrain.ManualUpdate();
    }
    public void AddRemoveRenderLayers(LayerMask layerToAdd, LayerMask layerToRemove)
    {
        _mainCam.cullingMask += layerToAdd;
        _mainCam.cullingMask -= layerToRemove;
    }

    private IEnumerator ResetCamera()
    {
        yield return new WaitForSeconds(_waitBeforeDistanceReset);
        _cameraDistance = _defaultCameraDistance;
        _shooting = false;
    }
}

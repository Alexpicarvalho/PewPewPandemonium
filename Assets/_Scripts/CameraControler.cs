using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using Cinemachine;
using Fusion;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using ExitGames.Client.Photon.StructWrapping;

public class CameraControler : MonoBehaviour
{
    float _cameraDistance;
    [SerializeField] float _defaultCameraDistance;
    [SerializeField] float _shootingCameraDistance;
    [SerializeField] float _confirmStopShooting = .7f;
    [SerializeField] PlayerCombatHandler _combatHandler;
    private CinemachineVirtualCamera _cinCam;
    bool _shooting = false;
    private Camera _mainCam;
    private CinemachineBrain _cmBrain;
    private Transform _player;
    float _stopShootingTimer = 0;

    [Header("Camera Feel")]
    [SerializeField] private float _minZaxisOffset;
    [SerializeField] private float _maxZaxisOffset;
    [SerializeField] private float _zeroOffsetRadius;
    [SerializeField] private float _maxOffsetRadius;
    [SerializeField] private Volume _postProcessing;
    private ColorAdjustments _colorAdj;
    private float _ZaxisOffset;
    Vector2 _screenCenter;
    CinemachineFramingTransposer _transposer;

    private void Start()
    {
        _cinCam = GetComponent<CinemachineVirtualCamera>();
        _transposer = _cinCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        _cameraDistance = _defaultCameraDistance;
        _mainCam = Camera.main;
        _cmBrain = _mainCam.GetComponent<CinemachineBrain>();
        _screenCenter = new Vector2(Screen.width / 2, Screen.height / 1.1f);
        //_player = transform.parent;


    }

    public void SetFollowTarget(Transform target)
    {
        if (_cinCam.Follow != null) return;
        _cinCam.Follow = target;
        _combatHandler = target.GetComponent<PlayerCombatHandler>();
    }

    private void Update()
    {
        _stopShootingTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            _stopShootingTimer = 0;
            _cameraDistance = _defaultCameraDistance + (_shootingCameraDistance - _defaultCameraDistance) * _combatHandler._gun._weaponSights;
        }
        else
        {
            if (_stopShootingTimer >= _confirmStopShooting) _cameraDistance = _defaultCameraDistance ;
        }

        Debug.Log("Camerda Distance is " + _cameraDistance);

        _transposer.m_CameraDistance = _cameraDistance;
    }

    public void LateUpdate()
    {
        //Debug.Log("Entered Network Update cycle");
        _cmBrain.ManualUpdate();
        SetViewOffset();
    }

    private void SetViewOffset()
    {
        float distanceToCenter = Vector2.Distance(Input.mousePosition, _screenCenter);

        if (distanceToCenter <= _zeroOffsetRadius) _ZaxisOffset = _minZaxisOffset;
        else
        {
            float t = Mathf.InverseLerp(_zeroOffsetRadius, _maxOffsetRadius, distanceToCenter);
            _ZaxisOffset = Mathf.Lerp(_minZaxisOffset, _maxZaxisOffset, t); 
            _ZaxisOffset = Mathf.Clamp(_ZaxisOffset, _minZaxisOffset, _maxZaxisOffset);
        }
        _transposer.m_TrackedObjectOffset = Vector3.forward * _ZaxisOffset;
    }

    public void AddRemoveRenderLayers(LayerMask layerToAdd, LayerMask layerToRemove)
    {
        _mainCam.cullingMask += layerToAdd;
        _mainCam.cullingMask -= layerToRemove;
    }
}

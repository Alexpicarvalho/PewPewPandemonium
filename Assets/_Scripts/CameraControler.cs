using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using Cinemachine;


public class CameraControler : MonoBehaviour
{
    float _cameraDistance;
    [SerializeField] float _defaultCameraDistance;
    [SerializeField] float _shootingCameraDistance;
    [SerializeField] float _waitBeforeDistanceReset;
    private CinemachineVirtualCamera _cinCam;
    bool _shooting = false;

    private void Start()
    {
        _cinCam = GetComponent<CinemachineVirtualCamera>();
        _cameraDistance = _defaultCameraDistance;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _shooting = true;
            _cameraDistance = _shootingCameraDistance;
            StopCoroutine(ResetCamera());
        }
        else
        {
            StopCoroutine(ResetCamera());
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

    private IEnumerator ResetCamera()
    {
        yield return new WaitForSeconds(_waitBeforeDistanceReset);
        _cameraDistance = _defaultCameraDistance;
        _shooting = false;
    }
}

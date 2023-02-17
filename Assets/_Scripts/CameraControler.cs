using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using Cinemachine;


public class CameraControler : MonoBehaviour
{
    [SerializeField] [Range(6,20)] float _cameraDistance;
    [SerializeField] float _waitBeforeDistanceReset;
    private CinemachineVirtualCamera _cinCam;
    bool _shooting = false;

    private void Start()
    {
        _cinCam = GetComponent<CinemachineVirtualCamera>();
        _cameraDistance = 13;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _shooting = true;
            _cameraDistance = 15;
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
        _cameraDistance = 13;
        _shooting = false;
    }
}

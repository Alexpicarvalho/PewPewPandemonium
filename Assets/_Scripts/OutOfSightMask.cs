using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using System;

public class OutOfSightMask : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private int _minRaycastChecks;
    [SerializeField] private int _maxRaycastChecks;
    [SerializeField] private float _distanceForMinChecks;
    [SerializeField] private float _distanceForMaxChecks;
    private int _currentRaycastChecks;

    [SerializeField] private float _raycastAngle;
    public float _viewDistance { private get; set; }
    public LayerMask _targetMask { private get; set; }

    private List<Transform> _targetsInRange = new List<Transform>();
    private List<Transform> _targetsInRangeLastFrame = new List<Transform>();
    private List<Transform> _visibleTargets = new List<Transform>();
    private List<Transform> _invisibleTargets = new List<Transform>();



    private void Update()
    {
        CheckForTargets();
        CheckTargetLineOfSight();
        RenderVisibleTargets();

        _targetsInRangeLastFrame = _targetsInRange;
        _targetsInRange.Clear();
    }

    private void RenderVisibleTargets()
    {
        foreach (var target in _visibleTargets)
        {

        }
    }

    private void CheckTargetLineOfSight()
    {
        foreach (Transform target in _targetsInRange)
        {
            RaycastBackToPlayer(target);
        }
    }

    private void RaycastBackToPlayer(Transform caster)
    {
        Vector3 casterPos = new Vector3(caster.position.x, 1.5f, caster.position.z);
        Vector3 playerPos = new Vector3(_player.transform.position.x, 1.5f, _player.transform.position.z);
        Vector3 rayCastDir = (playerPos - casterPos).normalized;
        Ray ray = new Ray(casterPos, rayCastDir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider != _player)
            {
                _invisibleTargets.Add(caster);
            }
            else
            {
                _visibleTargets.Add(caster);
            }
        }
    }

    private void CheckForTargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _viewDistance);

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.layer == _targetMask && !_targetsInRange.Contains(collider.transform))
            {
                _targetsInRange.Add(collider.transform);
            }
        }
    }
}

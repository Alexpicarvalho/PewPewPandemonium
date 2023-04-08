using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(Object_ID))]
public class MinigunTurretGO : NetworkBehaviour
{
    [Header("Setup Variables")]
    [SerializeField] Transform _hand;
    [SerializeField] Transform _firePoint;
    [SerializeField] GunSO _gunRef;
    [SerializeField] float _activationDelay = 2f;
    [SerializeField] float _lifeTime;

    [Header("Combat Variables")]
    [SerializeField] LayerMask _targetLayers;
    [SerializeField] float _targetDetectionRange;
    [SerializeField] GameObject _rangeVisualisor;

    [Header("Visuals")]
    [SerializeField] GameObject _onDestroyExplosionVFX;

    [Header("References")]
    Object_ID _ID;
    LineRenderer _lineRenderer;

    [Header("Runtime Variables")]
    private GunSO _gun;
    private Transform _currentTarget;
    private float _distanceToCurrentTarget;
    private Vector3 _lastTargetPosition;
    private List<Transform> _targetsInRange = new List<Transform>();
    bool _active = false;



    private void Start()
    {
        Destroy(transform.parent.gameObject, _lifeTime + _activationDelay);
        Invoke("Activate", _activationDelay);
        _ID = GetComponent<Object_ID>();
        _lineRenderer = GetComponent<LineRenderer>();
        _gun = Instantiate(_gunRef);
        _gun.PlaceInHand(_hand);
        _gun.SetWeaponValues(_firePoint, _ID);
        _rangeVisualisor.transform.localScale = 0.3f * _targetDetectionRange * Vector3.one;
    }

    private void Update()
    {
        if (!_active) return;
        _gun.UpdateWeaponStatus();
        _targetsInRange.Clear();
        _currentTarget = null;
        LookForTargets();
        DefineClosestTarget();
        LookAtTarget();
        FireAtTarget();
        
    }

    void Activate()
    {
        _active = true;
        _rangeVisualisor.SetActive(true);
    }

    private void FireAtTarget()
    {
        if (_currentTarget == null) return;
        _gun.NormalShoot();
    }

    private void LookForTargets()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position + Vector3.down, _targetDetectionRange, _targetLayers);

        foreach (var target in possibleTargets)
        {
            var entetyID = target.transform.GetComponent<Object_ID>();
            if (!_targetsInRange.Contains(target.transform) && entetyID != null && entetyID.my_ID != _ID.my_ID)
            {
                _targetsInRange.Add(target.transform);
            }
        }
    }
    private void DefineClosestTarget()
    {
        if (_targetsInRange.Count == 0) return;

        float[] distances = new float[_targetsInRange.Count];
        int index = 0;
        foreach (var player in _targetsInRange)
        {
            distances[index] = Vector3.Distance(transform.position, player.transform.position);
            index++;
        }
        int targetIndex = Array.IndexOf(distances, distances.Min());
        _currentTarget = _targetsInRange[targetIndex];
        _distanceToCurrentTarget = distances[targetIndex];
    }

    private void LookAtTarget()
    {
        if (_currentTarget != null)
        {
            _lineRenderer.enabled = true;
            transform.LookAt(_currentTarget.transform.position + Vector3.up);
            _lineRenderer.SetPosition(0, _firePoint.position);
            _lineRenderer.SetPosition(1, _currentTarget.transform.position + Vector3.up);
            _lastTargetPosition = _currentTarget.position + Vector3.down;
        }
        else
        {
            //Vector3 waitLook = new Vector3(20f, _lastYRotation, 0f);
            transform.LookAt(_lastTargetPosition);
            _lineRenderer.enabled = false;
        }

    }

    private void OnDestroy()
    {
        Runner.Spawn(_onDestroyExplosionVFX, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, .4f);
       // Gizmos.DrawSphere(transform.position + Vector3.down, _targetDetectionRange);
    }
}

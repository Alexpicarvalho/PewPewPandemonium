using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using System;
using Fusion;

public class OutOfSightMask : NetworkBehaviour
{

    private GameObject _player;
    private Collider _playerCollider;
    [Header("Line of Sight Checker")]
    public float _viewDistance;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _noLoSBlock;
    [SerializeField] float _targetListClearFrequency;

    private float _timeSinceLastRefresh = 0;

    private List<Transform> _targetsInRange = new List<Transform>();

    private void Awake()
    {

    }

    public override void Spawned()
    {
        base.Spawned();
        if (!Object.HasInputAuthority) this.enabled = false;
        _player = transform.gameObject;
        _playerCollider = _player.GetComponent<Collider>();

    }

    private void Update()
    {
        _targetsInRange.Clear();
        CheckForTargets();
        CheckTargetLineOfSight();
    }
    private void CheckForTargets()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _viewDistance, _targetMask);

        foreach (var collider in hitColliders)
        {
            if (collider.GetComponent<IHideable>() == null)
            {
                // Debug.LogError("CAREFUL, ENTETY IN TARGET LAYER DOESN'T HAVE HIDEABLE COMPONENT! Entety name: " + collider.name);
            }
            if (!_targetsInRange.Contains(collider.transform))
            {
                _targetsInRange.Add(collider.transform);
            }
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
        Vector3 casterPos = new Vector3(caster.position.x, 1f, caster.position.z);
        Vector3 playerPos = new Vector3(_player.transform.position.x, 1f, _player.transform.position.z);
        Vector3 rayCastDir = (playerPos - casterPos).normalized;
        Ray ray = new Ray(casterPos, rayCastDir);
        RaycastHit hit;
        IHideable target = caster.GetComponent<IHideable>();

        if (target == null) return;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _noLoSBlock))
        {
            if (hit.collider != _playerCollider || !target.Hiden())
            {
                target.HideMe();
            }
            else if (hit.collider == _playerCollider && target.Hiden())
            {
                target.RevealMe();
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (var target in _targetsInRange)
        {
            Ray r = new Ray(target.position, (_player.transform.position + Vector3.up - target.transform.position) * 20000);
            Gizmos.DrawRay(r);
        }

    }



}

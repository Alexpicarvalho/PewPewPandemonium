using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;


[CreateAssetMenu(menuName = "Weapons/Grenade", fileName = "GrenadeSO")]
public class GrenadeSO : Item
{
    [Header("Physical/Visual Properties")]
    [SerializeField] GameObject _grenadeGO;

    [Header("Grenade Path Visuals")]
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] [Range(10, 100)] int _linePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] float _timeBetweenPoints = 0.1f;

    [Header("Individual Attributes")]
    [SerializeField] float _throwForce;
    [SerializeField] Vector3 _throwAngle;
    [SerializeField] float _heightDisplacement;
    public float _cooldown;
    public string _animatorTrigger;

    [Header("Runtime Properties")]
    private Transform _firePoint;
    private Rigidbody _grenadeRb;
    [Range(-1, 1)] public int gravityDirection;
    //[Header("Optional Parameters")]


    public virtual void SetValues(Transform firePoint)
    {
        _firePoint = firePoint;
        _grenadeRb = _grenadeGO.GetComponent<Rigidbody>();
        _lineRenderer = firePoint.GetComponent<LineRenderer>();
    }

    public virtual void Throw(Vector3 target)
    {
        var grenadeGO = Instantiate(_grenadeGO, _firePoint.position, Quaternion.identity);
        var grenadeScript = grenadeGO.GetComponent<Grenade>();
        grenadeScript.CalculateThrowVelocity(target, _heightDisplacement);
        grenadeScript.Launch();
    }

    //public void EnableIndicator()
    //{
    //    _lineRenderer.enabled = true;
    //    Debug.Log("Enabled Line Renderer");
    //}

    //public void DisableIndicator()
    //{
    //    _lineRenderer.enabled = false;
    //    Debug.Log("Disabled Line Renderer");
    //}
    //public void DrawProjection()
    //{
    //    Debug.Log("Line Renderering");
    //    _lineRenderer.positionCount = Mathf.CeilToInt(_linePoints / _timeBetweenPoints + 1);

    //    Vector3 startPos = _firePoint.position;
    //    Vector3 startVelocity = _throwForce * _throwAngle / _grenadeRb.mass;

    //    int i = 0;
    //    _lineRenderer.SetPosition(i, startPos);

    //    for (float time = 0; time < _linePoints; time += _timeBetweenPoints)
    //    {
    //        i++;
    //        Vector3 point = startPos + time * startVelocity;
    //        point.y = startPos.y + startVelocity.y * time * (gravityDirection * Physics.gravity.y / 2f * time * time);

    //        _lineRenderer.SetPosition(i, point);
    //    }
    //}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[CreateAssetMenu(menuName = "Weapons/Skill", fileName = "Skill")]
public class WeaponSkillSO : ScriptableObject
{
    [Header("General Properties")]
    [SerializeField] public float _cooldown;
    [SerializeField] public string _skillDescription;
    [SerializeField] public SkillState _skillState;
    [SerializeField] public string _animatorTrigger;
    [SerializeField] public GameObject _skillVFX;

    [Header("Optional Properties")]
    [SerializeField] public float _activeTime;
    [SerializeField] public float _damage;
    [SerializeField] public Transform _firePoint;


    [Header("Visual Properties")]
    [SerializeField] private GameObject _visualIndicator;
    [SerializeField] public GameObject _castVFX;
    [SerializeField] public Vector3 _indicatorScale;
    [SerializeField] public Vector3 _indicatorPosition;
    [SerializeField] public Vector3 _indicatorRotation;


    [Header("Hidden/Runtime Properties")]
    [HideInInspector] public float _finalDamage; // may be obsolete
    private GameObject _indicatorClone;
    

    //Enums
    public enum SkillState { Ready, Active, OnCooldown}


    //Methods
    
    public void SetSkillValues(Transform firePoint = null, float damageMult = 1) 
    {
        _firePoint = firePoint;
        _damage = _damage * damageMult;
        if (_visualIndicator)
        {
            _indicatorClone = Instantiate(_visualIndicator, _firePoint.position, Quaternion.identity,_firePoint);
            _indicatorClone.transform.localScale = _indicatorScale;
            _indicatorClone.transform.rotation = Quaternion.Euler(_indicatorRotation);
            _indicatorClone.transform.localPosition = _indicatorPosition;
            _indicatorClone.SetActive(false);
        } 
    }

    public virtual void ShowSkillIndicator() 
    {
        /*Does it make sense to make this a generalized method or a case by case? */
        _indicatorClone?.SetActive(true);
    }
    public virtual void HideSkillIndicator() { _indicatorClone?.SetActive(false); }
    
    public virtual void ExecuteSpell(/*GameObject parent*/)
    {
        _indicatorClone.SetActive(false);
    }
    public virtual void StartCastingVFX() { }

    public virtual void DestroySpell() 
    {
        Destroy(_indicatorClone);
    }
}

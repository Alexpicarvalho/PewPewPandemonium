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
    [HideInInspector] public float _damage;
    [SerializeField] public Transform _firePoint;


    [Header("Visual Properties")]
    [SerializeField] private GameObject _visualIndicator;
    [SerializeField] private GameObject _castVFX;

    [Header("Hidden/Runtime Properties")]
    [HideInInspector] public float _finalDamage; // may be obsolete
    private GameObject _indicatorClone;
    

    //Enums
    public enum SkillState { Ready, Active, OnCooldown}


    //Methods
    
    public void SetSkillValues(/*GameObject parent,*/ Transform firePoint = null) 
    {
        _firePoint = firePoint;
        if (_visualIndicator)
        {
            _indicatorClone = Instantiate(_visualIndicator, _firePoint.position, Quaternion.identity);
            _indicatorClone.transform.parent = _firePoint;
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
}

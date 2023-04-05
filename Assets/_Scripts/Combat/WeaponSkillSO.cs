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
    [SerializeField] public Transform _firePoint;


    [Header("Visual Properties")]
    [SerializeField] private GameObject _visualIndicator;
    [SerializeField] public GameObject _castVFX;
    [SerializeField] public Vector3 _indicatorScale;
    [SerializeField] public Vector3 _indicatorPosition;
    [SerializeField] public Vector3 _indicatorRotation;
    [SerializeField] public Texture _skillIcon;

    [Header("Damage Properties")]
    public float _amount;
    public float _pushForce;
    [Range(1, 99)] public int _tickAmount = 1;
    public float _damageOverTimeDuration;
    public Damage _damage;


    [Header("Hidden/Runtime Properties")]
    public float _finalDamage; // may be obsolete
    /*[HideInInspector]*/ public float _timeSinceActivation; 
    /*[HideInInspector]*/ public float _timeSinceLastUse; 
    private GameObject _indicatorClone;

    


    //Enums
    public enum SkillState { Ready, Casting, Active, OnCooldown}


    //Methods
    
    public void SetSkillValues(Transform firePoint = null, float damageMult = 1) 
    {
        _firePoint = firePoint;
        _damage = new Damage(_amount * damageMult, _pushForce, _tickAmount, _damageOverTimeDuration);
        if (_visualIndicator)
        {
            _indicatorClone = Instantiate(_visualIndicator, _firePoint.position, Quaternion.identity,_firePoint.parent);
            _indicatorClone.transform.localScale = _indicatorScale;
            _indicatorClone.transform.rotation = Quaternion.Euler(_indicatorRotation);
            _indicatorClone.transform.localPosition = _indicatorPosition;
            _indicatorClone.SetActive(false);
        } 
    }


    public void UpdateSkillStatus()
    {

        if(_skillState == SkillState.Active)
        {
            _timeSinceActivation += Time.deltaTime;
            if(_timeSinceActivation >= _activeTime)
            {
                _skillState = SkillState.OnCooldown;
                _timeSinceActivation = 0;
            } 
        }

        if(_skillState == SkillState.OnCooldown)
        {
            _timeSinceLastUse += Time.deltaTime;
            if(_timeSinceLastUse >= _cooldown)
            {
                _skillState = SkillState.Ready;
                _timeSinceLastUse = 0;
            } 
        }
    }
    public virtual void ShowSkillIndicator() 
    {
        /*Does it make sense to make this a generalized method or a case by case? */
        _indicatorClone?.SetActive(true);
    }
    public virtual void HideSkillIndicator() { _indicatorClone?.SetActive(false); }
    
    public virtual void ExecuteSpell(/*GameObject parent*/ Vector3 _target = new Vector3())
    {
        if (_skillState != SkillState.Casting) return;
        _skillState = SkillState.Active;
        if(_indicatorClone != null)_indicatorClone.SetActive(false);
    }
    public virtual void StartCastingVFX() { }

    public virtual void DestroySpell() 
    {
        Destroy(_indicatorClone);
    }
}

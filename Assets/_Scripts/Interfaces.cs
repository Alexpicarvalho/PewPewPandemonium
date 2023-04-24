using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Interfaces
{ 
}

//[RequireComponent(typeof(Fusion.NetworkTransform))]
//[RequireComponent(typeof(Fusion.NetworkObject))]
public abstract class Damager : NetworkBehaviour
{ 
    [field : Header("Set Damage Values")]
    [Networked][field : SerializeField] float _amount { get; set; }
    [SerializeField] float _addForce;
    [SerializeField] [Range(1,99)] int _tickAmount;
    [SerializeField] float _damageOverTimeDuration;

    /*[Networked]*/ public Damage _damage { get; set; }

    public virtual void SetDamage(Damage newDamage = null)
    {
        //if (_damage != null) return;

        if (newDamage != null) _damage = newDamage;
        else _damage = new Damage(_amount, _addForce, _tickAmount, _damageOverTimeDuration);

    }
}
public interface IHitable
{
    void HandleHit(Damage damage);
}

public interface IDamager
{
    //Both skill effect and scriptable object have this, the latter sets the former
    public Damage Damage { get; set; }

    public void SetDamage(Damage newDamage);
    
}

public interface IHideable
{
    public void HideMe();
    public void RevealMe();
    public bool Hiden();
}


public enum Rarity { Common, Rare, Epic, Legendary }
public enum WeaponTier { Tier3, Tier2, Tier1, Special }

public class Damage
{
    [Networked] public float _amount { get; set; }
    [Networked] public float _addForce {get; set;}
    [Networked] public int _tickAmount {get; set;}
    [Networked] public float _damageOverTimeDuration {get; set;}

    // Fed Values Constructor
    public Damage(float amount, float addForce = 0, int tickNum = 1, float damageOTDuration = 0)
    {
        _amount = amount;
        _addForce = addForce;
        _tickAmount = tickNum;
        _damageOverTimeDuration = damageOTDuration;
    }
}

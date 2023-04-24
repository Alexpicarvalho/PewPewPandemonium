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
    [Header("Set Damage Values")]
    [SerializeField] float _amount;
    [SerializeField] float _addForce;
    [SerializeField] [Range(1,99)] int _tickAmount;
    [SerializeField] float _damageOverTimeDuration;

    public Damage _damage;

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
    public float _amount;
    public float _addForce;
    public int _tickAmount;
    public float _damageOverTimeDuration;

    // Fed Values Constructor
    public Damage(float amount, float addForce = 0, int tickNum = 1, float damageOTDuration = 0)
    {
        _amount = amount;
        _addForce = addForce;
        _tickAmount = tickNum;
        _damageOverTimeDuration = damageOTDuration;
    }
}

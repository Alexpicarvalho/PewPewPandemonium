using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interfaces
{ 
}
public interface IHitable
{
    void HandleHit(Damage damage);
}


public enum Rarity { Common, Uncomon, Rare }
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

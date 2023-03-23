using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Perk : ScriptableObject
{
    public string _name;
    public string _description;
    public Texture _perkIcon;
    [Range(0, 3)] public int _points = 0;

    public virtual void LevelUp() 
    {
        if (_points == 3) return;
        _points++; 
    }

    public virtual void UpdateEffects() { }
}

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
    [HideInInspector] public CameraControler _cameraControler;
    public bool _reachedMaxLevel = false;
    public virtual void GetCamController(CameraControler camController) { _cameraControler = camController; }
    public virtual void LevelUp()
    {
        if (_reachedMaxLevel) return;
        _points++;
        
    }

    public virtual void UpdateEffects(GameObject player) 
    {
        if (_reachedMaxLevel) return ;
        if (_points == 3) _reachedMaxLevel = true;
    }
}

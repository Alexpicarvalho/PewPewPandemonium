using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;



[CreateAssetMenu(menuName = "Item", fileName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] public string _name;
    [SerializeField] public string _description;
    [SerializeField] public Texture _icon;
}

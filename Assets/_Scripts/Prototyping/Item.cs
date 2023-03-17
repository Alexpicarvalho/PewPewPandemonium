using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;



[CreateAssetMenu(menuName = "Item", fileName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] string _description;
    [SerializeField] Texture _icon;
}

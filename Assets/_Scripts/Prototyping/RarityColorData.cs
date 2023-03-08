using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Color/ColorData", fileName = "colorData")]
public class RarityColorData : ScriptableObject
{
    public Rarity rarity;
    public Color _color;
}

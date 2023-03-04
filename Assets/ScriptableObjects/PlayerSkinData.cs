using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkinData", fileName = "skinData")]
public class PlayerSkinData : ScriptableObject
{
    public Mesh _bodyMesh;
    public Mesh _headMesh;

}

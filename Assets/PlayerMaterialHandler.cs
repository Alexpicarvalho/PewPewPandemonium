using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PlayerMaterialHandler : MonoBehaviour
{
    private SkinnedMeshRenderer _bodyRenderer;
    private SkinnedMeshRenderer _headRenderer;
    [SerializeField] private PlayerSkinData _skinData;

    private void Start()
    {
      _bodyRenderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
      _headRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        _bodyRenderer.sharedMesh = _skinData._bodyMesh;
        _headRenderer.sharedMesh = _skinData._headMesh;
    }
}

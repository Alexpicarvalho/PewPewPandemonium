using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Fusion;

public class PlayerMaterialHandler : NetworkBehaviour
{
    private SkinnedMeshRenderer _bodyRenderer;
    private SkinnedMeshRenderer _headRenderer;
    [SerializeField] private PlayerSkinData _skinData;

    [Header("Flash on Damage")]
    [SerializeField] private float _flashDuration;
    [SerializeField] private Material _flashMat;

    [Header("Hidden")]
    private float _timeSinceFlashStart = 0;
    private Material _bodyMat;
    private Material _headMat;

    public override void Spawned()
    {
        base.Spawned();
        if (!HasInputAuthority) return;
        RPC_ChangeSkin();

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_ChangeSkin()
    {
        _bodyRenderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        _headRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        _bodyRenderer.sharedMesh = _skinData._bodyMesh;
        _headRenderer.sharedMesh = _skinData._headMesh;
        _bodyMat = _bodyRenderer.materials[0];
        _headMat = _headRenderer.materials[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        { 
            FlashOnDamage();
        }
    }

    public void FlashOnDamage()
    {
        StartCoroutine(_FlashOnDamage());
    }

    private IEnumerator _FlashOnDamage()
    {

        _bodyRenderer.material = _flashMat;
        _headRenderer.material = _flashMat;
        Debug.Log("Pre");

        yield return new WaitForSeconds(_flashDuration);

        _bodyRenderer.material = _bodyMat;
        _headRenderer.material = _headMat;
        Debug.Log("Pos");
    }
}

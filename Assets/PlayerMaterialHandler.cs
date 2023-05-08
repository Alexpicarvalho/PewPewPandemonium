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
    private CharacterCostumization _characterCostumization;

    [Header("Flash on Damage")]
    [SerializeField] private float _flashDuration;
    [SerializeField] private Material _flashMat;

    [Header("Hidden")]
    private float _timeSinceFlashStart = 0;
    private Material _bodyMat;
    private Material _headMat;

    [Networked] private int _bodyIndex { get; set; }
    [Networked] private int _headIndex { get; set; }

    private void Awake()
    {
        _characterCostumization = GetComponent<CharacterCostumization>();
    }

    public override void Spawned()
    {
        base.Spawned();

        _bodyIndex = 0;
        _headIndex = 0;

        if (!HasInputAuthority) this.enabled = false;

        _bodyIndex = _skinData._bodyMeshIndex;
        _headIndex = _skinData._headMeshIndex;

        RPC_ChangeSkin();

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    void RPC_ChangeSkin()
    {
        _bodyRenderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        _headRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        _bodyRenderer.sharedMesh = _characterCostumization.bodyParts[_skinData._bodyMeshIndex].sharedMesh;
        _headRenderer.sharedMesh = _characterCostumization.headParts[_skinData._headMeshIndex].sharedMesh;
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

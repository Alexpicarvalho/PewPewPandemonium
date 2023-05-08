using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterCostumization : NetworkBehaviour
{
    [SerializeField] public SkinnedMeshRenderer[] bodyParts;
    [SerializeField] public SkinnedMeshRenderer[] headParts;

    [SerializeField] float _dissolveTime;
    [SerializeField] float _undissolveTime;

    [SerializeField] PlayerSkinData _skinDataOutput;

    private Mesh[] bodySkins = new Mesh[20];
    private Mesh[] headSkins = new Mesh[20];
    private int currentBodyIndex = 0;
    private int currentHeadIndex = 0;
    private SkinnedMeshRenderer currentBody;
    private SkinnedMeshRenderer currentHead;
    [SerializeField] private Material _bodyMaterial;
    [SerializeField] private Material _headMaterial;
    bool _isDissolvingBody = false;
    bool _isDissolvingHead = false;
    // Start is called before the first frame update
    void Start()
    {

        currentBody = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        currentHead = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        //_bodyMaterial = currentBody.GetComponent<Material>();
        //_headMaterial = currentHead.GetComponent<Material>();
        _bodyMaterial = currentBody.materials[0];
        _headMaterial = currentHead.materials[0];
        GetSkins();
    }

    private void GetSkins()
    {
        int i = 0;
        int j = 0;
        foreach (var item in bodyParts)
        {
            bodySkins[i] = bodyParts[i].sharedMesh;
            i++;
        }
        foreach (var item in headParts)
        {
            headSkins[j] = headParts[j].sharedMesh;
            j++;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwapBodyPiece(true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwapBodyPiece(false);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwapHeadPiece(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SwapHeadPiece(false);
        }
    }

    public void SwapBodyPiece(bool next)
    {
        if (next && currentBodyIndex < bodySkins.Length - 1) currentBodyIndex++;
        else if (!next && currentBodyIndex > 0) currentBodyIndex--;
        else return;
        Invoke("ChangeBody", _dissolveTime);
        if (!_isDissolvingBody) StartCoroutine(Dissolve(_bodyMaterial, true));
    }
    void ChangeBody()
    {
        currentBody.sharedMesh = bodySkins[currentBodyIndex];
        _skinDataOutput._bodyMeshIndex = currentBodyIndex;
    }

    public void SwapHeadPiece(bool next)
    {
        if (next && currentHeadIndex < headSkins.Length - 1) currentHeadIndex++;
        else if (!next && currentHeadIndex > 0) currentHeadIndex--;
        else return;
        Invoke("ChangeHead", _dissolveTime);
        if (!_isDissolvingHead) StartCoroutine(Dissolve(_headMaterial, false));

    }
    void ChangeHead()
    {
        currentHead.sharedMesh = headSkins[currentHeadIndex];
        _skinDataOutput._headMeshIndex = currentHeadIndex;
    }

    IEnumerator Dissolve(Material dissolveMat, bool isBody)
    {
        if (isBody) _isDissolvingBody = true;
        else _isDissolvingHead = true;
        float startTime = Time.time;
        float time = 0;
        while (Time.time < startTime + _dissolveTime)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / _dissolveTime);
            dissolveMat.SetFloat("_Dissolve", Mathf.Lerp(0, 1, t));
            yield return null;
        }
        StartCoroutine(Undissolve(dissolveMat, isBody));
    }
    IEnumerator Undissolve(Material undissolveMat, bool isBody)
    {
        float startTime = Time.time;
        float time = 0;
        while (Time.time < startTime + _dissolveTime)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / _dissolveTime);
            undissolveMat.SetFloat("_Dissolve", Mathf.Lerp(1, 0, t));
            yield return null;
        }
        if (isBody) _isDissolvingBody = false;
        else _isDissolvingHead = false;
    }

    private void OnApplicationQuit()
    {
        _bodyMaterial.SetFloat("_Dissolve", 0);
        _headMaterial.SetFloat("_Dissolve", 0);
    }
}

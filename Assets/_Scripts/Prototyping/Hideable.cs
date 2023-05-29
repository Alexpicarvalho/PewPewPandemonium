using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Hideable : NetworkBehaviour, IHideable
{

    [SerializeField] private List<Renderer> _myRenderers = new List<Renderer>();
    [SerializeField] private List<GameObject> _hideParts = new List<GameObject>();
    [SerializeField] private bool _hiden = false;

    public bool _isLocal;

    public override void Spawned()
    {
        base.Spawned();
        StartCoroutine(GetAllRenderingInfo());
    }

    IEnumerator GetAllRenderingInfo()
    {
        yield return new WaitForSeconds(.2f);
        GetRenderers();
        if (Object.HasInputAuthority) RevertIssues();
    }
    private void GetRenderers()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (!_myRenderers.Contains(renderer)) _myRenderers.Add(renderer);
        }
    }

    private void RevertIssues()
    {
        RevealMe();
        _isLocal = true;
        Destroy(this);

    }

    public void UpdateRenderers()
    {
        if (_isLocal) return;
        GetRenderers();
    }

    public void HideMe()
    {
        if (_isLocal) return;
        _hiden = true;
        DisableRendering();
    }

    public void RevealMe()
    {
        if (_isLocal) return;
        _hiden = false;
        EnableRendering();
    }

    bool IHideable.Hiden()
    {
        return _hiden;
    }
    private void DisableRendering()
    {
        foreach (Renderer item in _myRenderers)
        {
            if (item) item.enabled = false;
        }

        foreach (GameObject item in _hideParts)
        {
            if (item) item.SetActive(false);
        }
    }
    private void EnableRendering()
    {
        foreach (Renderer item in _myRenderers)
        {
            item.enabled = true;
        }

        foreach (GameObject item in _hideParts)
        {
            item.SetActive(true);
        }
    }
}

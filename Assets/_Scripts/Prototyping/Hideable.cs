using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideable : MonoBehaviour, IHideable
{

    [SerializeField] private List<Renderer> _myRenderers = new List<Renderer>();
    [SerializeField] private List<GameObject> _hideParts = new List<GameObject>();
    [SerializeField] private bool _hiden = false;

    private void Awake()
    {
        GetRenderers();
    }

    private void GetRenderers()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            _myRenderers.Add(renderer);
        }
    }

    public void HideMe()
    {
        _hiden = true;
        DisableRendering();
    }

    public void RevealMe()
    {
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
            item.enabled = false;
        }

        foreach (GameObject item in _hideParts)
        {
            item.SetActive(false);
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

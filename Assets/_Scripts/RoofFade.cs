using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoofFade : MonoBehaviour
{
    [Range(0, 1)] public float _minFadeAlpha;
    [SerializeField] private Material _transparentMat;
    public float _fadeTime = 1f;

    private MeshRenderer _renderer;
    private Material _material;
    private Color _initialColor;
    bool _isTransparent = false;

    private void Start()
    {
        //transform.GetComponent<BoxCollider>().isTrigger = true;
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.material;
        _initialColor = _transparentMat.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Make Transparent
            StartCoroutine(MakeTransparent());
            _isTransparent = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MakeOpaque());
            _isTransparent = false;
        }
    }

    IEnumerator MakeTransparent()
    {
        _renderer.material = _transparentMat;
        float startTime = Time.time;
        float time = 0;

        while (Time.time < startTime + _fadeTime)
        {
            time += Time.deltaTime;
            float t = time / _fadeTime;
            _renderer.material.color = new Color(_initialColor.r, _initialColor.g, _initialColor.b, Mathf.Lerp(_initialColor.a, _minFadeAlpha, t));
            yield return null;
        }
    }

    IEnumerator MakeOpaque()
    {
        float startTime = Time.time;
        float time = 0;

        while (Time.time < startTime + _fadeTime)
        {
            time += Time.deltaTime;
            float t = time / _fadeTime;
            _renderer.material.color = new Color(_initialColor.r, _initialColor.g, _initialColor.b, Mathf.Lerp(_minFadeAlpha, _initialColor.a, t));
            yield return null;
        }
        _renderer.material = _material;
    }

}

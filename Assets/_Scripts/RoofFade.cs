using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(BoxCollider))]
public class RoofFade : NetworkBehaviour
{
    [Range(0, 1)] public float _minFadeAlpha;
    [SerializeField] private Material _transparentMat;
    public float _fadeTime = 1f;

    private MeshRenderer _renderer;
    private Material _material;
    private Color _initialColor;
    bool _isTransparent = false;

    public string _lastStateWas = "";
    private bool _buffering = false;

    private void Start()
    {
        //transform.GetComponent<BoxCollider>().isTrigger = true;
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.material;
        _initialColor = _transparentMat.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<NetworkObject>().HasInputAuthority)
        {
            //Make Transparent
            //StartCoroutine(MakeTransparent());
            //StartCoroutine(NetworkCorrectionSafeguard(true));
            _renderer.enabled = false;
            _isTransparent = true;
            _lastStateWas = "Entered";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<NetworkObject>().HasInputAuthority)
        {
            //StartCoroutine(MakeOpaque());
            //StartCoroutine(NetworkCorrectionSafeguard(false));
            _renderer.enabled = true;
            _isTransparent = false;
            _lastStateWas = "Exited";
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player") && other.GetComponent<NetworkObject>().HasInputAuthority)
    //        StopCoroutine(MakeOpaque());
    //}

    IEnumerator NetworkCorrectionSafeguard(bool entered)
    {
        if (_buffering) yield break;
        _buffering = true;
        yield return new WaitForSeconds(.1f);
        if (entered) StartCoroutine(MakeTransparent());
        else StartCoroutine(MakeOpaque());
        _buffering = false;
    }

    IEnumerator MakeTransparent()
    {
        StopCoroutine(MakeOpaque());
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
        StopCoroutine(MakeTransparent());
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

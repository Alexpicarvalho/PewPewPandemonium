using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Shoeprint : NetworkBehaviour
{
    public float _delayBeforeFade;
    public float _fadeDuration;
    public LayerMask _layerMask;
    private Renderer _renderer;
    private Material _mat;
    private GameObject _parent;
    private NetworkObject _object;
    private void Start()
    {
        SetColor();
        _parent = transform.parent.gameObject;
        _object = _parent.GetComponent<NetworkObject>();
        _renderer = GetComponent<Renderer>();
        _mat = _renderer.material;
        StartCoroutine(FadeFootPrint());
    }

    public override void Spawned()
    {
        base.Spawned();
        
    }

    private void SetColor()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, _layerMask))
        {
            hit.collider.TryGetComponent<Terrain>(out Terrain terrain);

            if (!terrain) return;


        }
        else Debug.Log("Raycast No Hit");
    }

    IEnumerator FadeFootPrint()
    {
        yield return new WaitForSeconds(_delayBeforeFade);
        float startTime = Time.time;
        float time = 0;
        float startAlpha = _mat.color.a;
        while (Time.time < startTime + _fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / _fadeDuration);
            _mat.SetColor("_BaseColor", new Color(_mat.color.r, _mat.color.g, _mat.color.b, Mathf.Lerp(startAlpha, 0, t)));
            _parent.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            yield return null;
        }
        if (HasStateAuthority) Runner.Despawn(_object);
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Gizmos.DrawRay(ray);
    }
}

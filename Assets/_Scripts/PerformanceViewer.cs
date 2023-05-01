using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public class PerformanceViewer : NetworkBehaviour
{
    private TextMeshProUGUI fpsCounter;
    [SerializeField] private TextMeshProUGUI pingCounter;
    public int avgFrameRate;
    public float _viewRefreshRate = .1f;
    private float _timeSinceLastRefresh = 0f;
    private float _timeSinceLastCount = 0f;
    NetworkRunner _networkRunner;
    double pingCount;
    private void Awake()
    {
        fpsCounter = GetComponentInChildren<TextMeshProUGUI>();
        pingCounter.enabled = false;
    }

    public override void Spawned()
    {
        base.Spawned();
        _networkRunner = FindObjectOfType<NetworkRunner>();
    }

    public void Update()
    {
        _timeSinceLastRefresh += Time.unscaledDeltaTime;
        _timeSinceLastCount += Time.unscaledDeltaTime;

        if (_timeSinceLastRefresh >= _viewRefreshRate)
        {
            float current = 0;
            current = (int)(1f / Time.unscaledDeltaTime);
            avgFrameRate = (int)current;
            fpsCounter.text = avgFrameRate.ToString() + " FPS";
            _timeSinceLastRefresh = 0;
        }

        //pingCount += _networkRunner.GetPlayerRtt(Runner.LocalPlayer);

        //if(_timeSinceLastCount >= 1.0f)
        //{
        //    pingCounter.text = pingCount.ToString() + " ms";
        //    pingCount = 0;
        //    _timeSinceLastCount = 0;
        //}
    }
}



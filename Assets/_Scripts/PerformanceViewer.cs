using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerformanceViewer : MonoBehaviour
{
    private TextMeshProUGUI fpsCounter;
    public int avgFrameRate;
    public float _viewRefreshRate = .1f;
    private float _timeSinceLastRefresh = 0f;

    private void Awake()
    {
        fpsCounter = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Update()
    {
        _timeSinceLastRefresh += Time.unscaledDeltaTime;

        if (_timeSinceLastRefresh >= _viewRefreshRate)
        {
            float current = 0;
            current = (int)(1f / Time.unscaledDeltaTime);
            avgFrameRate = (int)current;
            fpsCounter.text = avgFrameRate.ToString() + " FPS";
            _timeSinceLastRefresh = 0;
        }
    }
}



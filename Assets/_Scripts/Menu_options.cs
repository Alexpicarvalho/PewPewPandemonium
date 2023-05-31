using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_options : MonoBehaviour
{
    public Toggle fullScreen, vsync;
    // Start is called before the first frame update
    void Start()
    {
        fullScreen.isOn = Screen.fullScreen;
        if (QualitySettings.vSyncCount == 0)
        {
            vsync.isOn = false;
        }
        else vsync.isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullScreen.isOn;
        if (vsync.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else QualitySettings.vSyncCount = 0;
    }

}

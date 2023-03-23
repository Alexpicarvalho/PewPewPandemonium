using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetPerkVisual : MonoBehaviour
{
    [SerializeField] Perk _perk;
    [SerializeField] RawImage _icon;
    [SerializeField] TextMeshProUGUI _levelText;
    private void Start()
    {
        _icon.texture = _perk._perkIcon;
    }

    private void Update()
    {
        _levelText.text = _perk._points.ToString();
    }
}

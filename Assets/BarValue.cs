using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarValue : MonoBehaviour
{

    private General_Stats _playerStats;
    [SerializeField] ValueType _barType;
    [SerializeField] Color _badColor;
    [SerializeField] Color _goodColor;

    Slider _barSlider;
    Image _barImage;

    enum ValueType { HPBar, ShieldBar }
    // Start is called before the first frame updates
    void Start()
    {
        _playerStats = GetComponentInParent<General_Stats>();
        _barSlider = GetComponent<Slider>();
        _barImage = transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (_barType)
        {
            case ValueType.HPBar:
                _barSlider.value = _playerStats.CurrentHp / _playerStats.MaxHP;
                break;
            case ValueType.ShieldBar:
                _barSlider.value = _playerStats.CurrentShield / _playerStats.MaxShield;
                break;
        }

        Color newColor = Color.Lerp(_badColor, _goodColor, _barSlider.value);
        _barImage.color = newColor;

    }
}

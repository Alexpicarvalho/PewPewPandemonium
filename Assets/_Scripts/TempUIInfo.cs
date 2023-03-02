using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TempUIInfo : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] TextMeshProUGUI _ammoText;
    [SerializeField] RawImage _weaponImage;
    [SerializeField] RawImage _offWeaponImage;
    private Image _border1;
    private Image _border2;
    private PlayerCombatHandler _playerCombatHandler;
    private PlayerStats _playerStats;
    private GunSO _lastFrameWeapon;
    public Animator _weaponUIAnimator;
    bool firstFrame = true;
    // Start is called before the first frame update
    void Start()
    {
        _playerCombatHandler = _player.GetComponent<PlayerCombatHandler>();
        _playerStats = _player.GetComponent<PlayerStats>();
        _border1 = _weaponImage.transform.GetChild(0).GetComponent<Image>();
        _border2 = _offWeaponImage.transform.GetChild(0).GetComponent<Image>();

    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (_playerCombatHandler._weaponSlot2)
        {
            _weaponImage.enabled = true;
            _border1.enabled = true;
            _weaponImage.texture = _playerCombatHandler._weaponSlot2._weaponIcon;
            switch (_playerCombatHandler._weaponSlot2._weaponTier)
            {
                case WeaponTier.Tier3:
                    _border1.color = Color.green;
                    break;
                case WeaponTier.Tier2:
                    _border1.color = Color.blue;
                    break;
                case WeaponTier.Tier1:
                    _border1.color = new Color(1,0,1);
                    break;
                case WeaponTier.Special:
                    _border1.color = new Color(1,.5f,.5f);
                    break;
                default:
                    break;
            }
        }
        else
        {
            _weaponImage.enabled = false;
            _border1.enabled = false;
        } 
        if (_playerCombatHandler._weaponSlot1)
        {
            _border2.enabled = true;
            _offWeaponImage.enabled = true;
            _offWeaponImage.texture = _playerCombatHandler._weaponSlot1._weaponIcon;
            switch (_playerCombatHandler._weaponSlot1._weaponTier)
            {
                case WeaponTier.Tier3:
                    _border2.color = Color.green;
                    break;
                case WeaponTier.Tier2:
                    _border2.color = Color.blue;
                    break;
                case WeaponTier.Tier1:
                    _border2.color = new Color(1, 0, 1);
                    break;
                case WeaponTier.Special:
                    _border2.color = new Color(1, .5f, .5f);
                    break;
                default:
                    break;
            }
        }
        else
        {
            _offWeaponImage.enabled = false;
            _border2.enabled = false;
        } 




        if (_lastFrameWeapon && _lastFrameWeapon != _playerCombatHandler._gun)
        {
            _weaponUIAnimator.SetTrigger("Swap1");
            //_weaponUIAnimator.SetBool("Swap",true);
            //StartCoroutine(SetBoolFalse());
        }



        if (_playerCombatHandler._reloading)
        {
            _ammoText.color = Color.white;
            _ammoText.text = "RELOADING . . .";
        }
        else
        {
            _ammoText.color = Color.green;
            _ammoText.text = _playerCombatHandler._gun._bulletsInMag.ToString() +
                " / " + _playerCombatHandler._gun._magazineSize.ToString();
        }

        _lastFrameWeapon = _playerCombatHandler._gun;
    }

    IEnumerator SetBoolFalse()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Set Bool False");
        _weaponUIAnimator.SetBool("Swap", false);
    }

}

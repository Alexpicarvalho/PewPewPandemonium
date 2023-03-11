using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TempUIInfo : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] TextMeshProUGUI _ammoText;
    [SerializeField] Transform _weapon1UI;
    [SerializeField] Transform _weapon2UI;
    [SerializeField] Transform _skillUI;
    RawImage _weaponImage;
    RawImage _offWeaponImage;
    [SerializeField] RawImage _skillImage;
    [SerializeField] Image _cooldownMask;
    TextMeshProUGUI _cooldownText;
    private Image _border1;
    private Image _border2;
    private PlayerCombatHandler _playerCombatHandler;
    private General_Stats _playerStats;
    private GunSO _lastFrameWeapon;
    public Animator _weaponUIAnimator;
    public Animator _skillUIAnimator;
    bool firstFrame = true;
    // Start is called before the first frame update
    void Start()
    {
        _playerCombatHandler = _player.GetComponent<PlayerCombatHandler>();
        _playerStats = _player.GetComponent<General_Stats>();
        _weaponImage = _weapon1UI.GetChild(2).GetComponent<RawImage>();
        _offWeaponImage = _weapon2UI.GetChild(2).GetComponent<RawImage>();
        _border1 = _weapon1UI.transform.GetChild(1).GetComponent<Image>();
        _border2 = _weapon2UI.transform.GetChild(1).GetComponent<Image>();
        _cooldownText = _cooldownMask.GetComponentInChildren<TextMeshProUGUI>();
        _skillUIAnimator = _skillUI.GetComponent<Animator>();
        _skillImage.texture = _playerCombatHandler._weaponSlot2._weaponSkill._skillIcon;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (_playerCombatHandler._weaponSlot2)
        {
            _weapon1UI.gameObject.SetActive(true);
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
            _weapon1UI.gameObject.SetActive(false);
        } 
        if (_playerCombatHandler._weaponSlot1)
        {
            _weapon2UI.gameObject.SetActive(true);
            _offWeaponImage.texture = _playerCombatHandler._weaponSlot1._weaponIcon;
            //_skillImage.texture = _playerCombatHandler._weaponSlot1._weaponSkill._skillIcon;
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
            _weapon2UI.gameObject.SetActive(false);
        }


        //Skill Cooldown Zone
        WeaponSkillSO _currentWeaponSkill = _playerCombatHandler._gun._weaponSkill;
        
        if (_currentWeaponSkill._skillState == WeaponSkillSO.SkillState.OnCooldown)
        {
            _cooldownMask.enabled = true;
            _cooldownText.text = ((int)(_currentWeaponSkill._cooldown - _currentWeaponSkill._timeSinceLastUse) + 1).ToString();
            _cooldownMask.fillAmount = 1 - (_currentWeaponSkill._timeSinceLastUse / _currentWeaponSkill._cooldown);
            
        }
        else if(_currentWeaponSkill._skillState == WeaponSkillSO.SkillState.Ready)
        {
            _cooldownMask.enabled = false;
            _cooldownText.text = "";
        }
        




        if (_lastFrameWeapon && _lastFrameWeapon != _playerCombatHandler._gun)
        {
            _weaponUIAnimator.SetTrigger("Swap1");
            _skillUIAnimator.SetTrigger("Flip");
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

    public void SwapSkillIcon()
    {
        _skillImage.texture = _playerCombatHandler._gun._weaponSkill._skillIcon;
    }

    IEnumerator SetBoolFalse()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Set Bool False");
        _weaponUIAnimator.SetBool("Swap", false);
    }

}

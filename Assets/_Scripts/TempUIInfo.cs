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
    private PlayerCombatHandler _playerCombatHandler;
    private Stats _playerStats;
    private GunSO _lastFrameWeapon;
    public Animator _weaponUIAnimator;
    bool firstFrame = true;
    // Start is called before the first frame update
    void Start()
    {
        _playerCombatHandler = _player.GetComponent<PlayerCombatHandler>();
        _playerStats = _player.GetComponent<Stats>();

    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (_playerCombatHandler._weaponSlot2)
        {
            _weaponImage.enabled = true;
            _weaponImage.texture = _playerCombatHandler._weaponSlot2._weaponIcon;
        } 
        else _weaponImage.enabled = false;
        if (_playerCombatHandler._weaponSlot1)
        {
            _offWeaponImage.enabled = true;
            _offWeaponImage.texture = _playerCombatHandler._weaponSlot1._weaponIcon;
        }
        else _offWeaponImage.enabled = false;




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

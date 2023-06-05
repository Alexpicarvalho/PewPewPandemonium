using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] Slider volumeSlider = null;
   // [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float defaultVolume = 1.0f;
    
    public void PlayGame() 
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("CombatScene");
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("mastervolume"))
        {
            PlayerPrefs.SetFloat("mastervolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void SetVolume (float volume)
    {
        AudioListener.volume = volumeSlider.value;
        volumeTextValue.text = volume.ToString("0.0");
        VolumeApply();

    }
    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("mastervolume", volumeSlider.value);
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("mastervolume");
    }


    //public IEnumerator ConfirmationBox()
    //{
    //    confirmationPrompt.SetActive(true);
    //    yield return new WaitForSeconds(2);
    //    confirmationPrompt.SetActive(false);
    //}

}

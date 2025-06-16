using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [SerializeField] private GameObject soundUI;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle bgmToggle;
    [SerializeField] private Toggle sfxToggle;
    private bool isUIOpen = false;

   
    private void Start()
    {
        SetUI(false);
        StartCoroutine(DelayedLoad());
    }

    private IEnumerator DelayedLoad()
    {
        yield return null;
        LoadSoundSetting();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetUI(!isUIOpen);
        }
    }

    private void SetUI(bool isOpen)
    {
        isUIOpen = isOpen;
        
        if (!isUIOpen)
        {
            SaveSoundSetting();
        }
        soundUI.SetActive(isUIOpen);

        Time.timeScale = isUIOpen ? 0 : 1;
        Cursor.lockState = isUIOpen ? CursorLockMode.Confined :  CursorLockMode.Locked;

    }

    private void SaveSoundSetting()
    {
        SaveManager.Instance.UpdateSoundSetting(bgmSlider.value, 
            SoundManager.Instance.IsBgmMute(), 
            sfxSlider.value, 
            SoundManager.Instance.IsSfxMute());
    }

    private void LoadSoundSetting()
    {
        var data = SaveManager.Instance.GetCurrentSaveData();
        
        bgmSlider.value = data.currentBgmVolume;
        sfxSlider.value = data.currentSfxVolume;
        bgmToggle.isOn = data.currentBgmMute;
        sfxToggle.isOn = data.currentSfxMute;
        
    }


    public void ExitButton()
    {
        SetUI(false);
    }

    public void ToggleBgm()
    {
        SoundManager.Instance.ToggleBgmMute();
    }

    public void ToggleSfx()
    {
        SoundManager.Instance.ToggleSfxMute();
    }

    public void SetBgmVolume()
    {
        SoundManager.Instance.SetBgmVolume(bgmSlider.value);
    }

    public void SetSfxVolume()
    {
        SoundManager.Instance.SetSfxVolume(sfxSlider.value);
    }
}

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
    
    SaveManager saveManager;
    SoundManager soundManager;
   
    private void Start()
    {
        saveManager = SaveManager.Instance;
        soundManager = SoundManager.Instance;
        LoadSoundSetting();
        SetUI(false);
        soundManager.PlayBgmLoop("DefaultBGM");
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
        saveManager.UpdateSoundSetting(bgmSlider.value, 
            soundManager.IsBgmMute(), 
            sfxSlider.value, 
            soundManager.IsSfxMute());
        
        saveManager.SaveOption();
    }

    private void LoadSoundSetting()
    {
        var data = saveManager.GetSoundOptionData();
        bgmSlider.value = data.currentBgmVolume;
        sfxSlider.value = data.currentSfxVolume;
        bgmToggle.isOn = data.currentBgmMute;
        sfxToggle.isOn = data.currentSfxMute;
        Debug.Log($"데이터 {data} 현재 볼륨{data.currentBgmVolume}");
    }


    public void ExitButton()
    {
        SetUI(false);
    }

    public void ToggleBgm()
    {
        soundManager.ToggleBgmMute();
    }

    public void ToggleSfx()
    {
        soundManager.ToggleSfxMute();
    }

    public void SetBgmVolume()
    {
        soundManager.SetBgmVolume(bgmSlider.value);
    }

    public void SetSfxVolume()
    {
        soundManager.SetSfxVolume(sfxSlider.value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [SerializeField] private GameObject soundUI;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    private bool isUIOpen = false;

   
    private void Start()
    {
        SetUI(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetUI(!isUIOpen);
        } }

    private void SetUI(bool isOpen)
    {
        isUIOpen = isOpen;
        
        soundUI.SetActive(isUIOpen);

        Time.timeScale = isUIOpen ? 0 : 1;
        Cursor.lockState = isUIOpen ? CursorLockMode.Confined :  CursorLockMode.Locked;
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

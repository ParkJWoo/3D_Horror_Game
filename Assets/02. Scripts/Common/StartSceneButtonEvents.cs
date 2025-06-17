using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StartSceneButtonEvents : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public string mainSceneName = "MainScene";
    public Button LoadButton;

    private void Start()
    {
        LoadButtonState();
    }

    //  시작 버튼에 연결
    public void OnStartButtonClicked()
    {
        Debug.Log("시작 버튼 클릭");

        if (sceneLoader != null)
        {
            GameManager.Instance.sceneLoader.MoveScene(mainSceneName);
        }

        else
        {
            Debug.LogWarning("[StartSceneButtonEvents] SceneLoader가 할당되지 않음!");
        }
    }

    //  게임 종료 버튼에 연결
    public void OnExitButtonClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //  불러오기 버튼에 연결
    public void OnLoadButtonClicked()
    {
        GameManager.Instance.sceneLoader.MoveScene(mainSceneName);
    }

    public void LoadButtonState()
    {
        if (!File.Exists(SaveManager.Instance.optionDataPath))
        {
            LoadButton.interactable = false;
        }

        else
        {
            LoadButton.interactable = true;
        }
    }
}

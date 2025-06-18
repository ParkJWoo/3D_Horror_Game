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

    private void Awake()
    {
    }

    private void Start()
    {
        sceneLoader = GameManager.Instance.sceneLoader;
        LoadButtonState();
    }

    //  시작 버튼에 연결
    public void OnStartButtonClicked()
    {
        //  씬 이동 전에 오브젝트 / 참조 정리
        if (GameManager.Instance != null)
        {
        }

        if (sceneLoader != null)
        {
            GameManager.Instance.isNewGame = true;
            sceneLoader.MoveScene(mainSceneName);
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
        //  씬 이동 전에 오브젝트 / 참조 정리
        if (GameManager.Instance != null)
        {
            //GameManager.Instance.ClearAllReferences();
        }

        if (sceneLoader != null)
        {
            GameManager.Instance.isNewGame = false;
            sceneLoader.MoveScene(mainSceneName);
        }

        else
        {
            Debug.LogWarning("[StartSceneButtonEvents] SceneLoader가 할당되지 않음!");
        }
    }

    public void LoadButtonState()
    {
        if (LoadButton == null)
        {
            Debug.LogWarning("[StartSceneButtonEvents] LoadButton 연결 안 됨!");
            return;
        }

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

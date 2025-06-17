using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneButtonEvents : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public string mainSceneName = "MainScene";

    //  시작 버튼에 연결
    public void OnStartButtonClicked()
    {
        if (sceneLoader != null)
        {
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
    }
}

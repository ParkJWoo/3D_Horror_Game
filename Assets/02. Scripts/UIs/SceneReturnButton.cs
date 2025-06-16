using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneReturnButton : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public string targetSceneName = "LoadingScene";

    public void ReturnToScene()
    {
        Debug.Log("[SceneReturnButton] ReturnToScene() 호출됨");

        if (sceneLoader == null)
        {
            Debug.LogWarning("[SceneReturnButton] SceneLoader 연결 안 됨");
            return;
        }

        sceneLoader.MoveScene(targetSceneName);
    }
}

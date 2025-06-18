using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneReturnButton : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public string targetSceneName = "StartScene";

    private void Awake()
    {
        //if (sceneLoader == null)
        //{
        //    sceneLoader = FindObjectOfType<SceneLoader>();

        //    if (sceneLoader == null)
        //    {
        //        Debug.LogWarning("[SceneReturnButton] 자동으로 SceneLoader를 찾지 못했습니다!");
        //    }
        //}
    }

    private void Start()
    {
        sceneLoader = GameManager.Instance.sceneLoader;
    }

    public void ReturnToScene()
    {
        Debug.Log("[SceneReturnButton] ReturnToScene() 호출됨");

        //  모든 참조 / 오브젝트 해제
        if (GameManager.Instance != null)
        {
            //GameManager.Instance.ClearAllReferences();
        }

        if (sceneLoader == null)
        {
            Debug.LogWarning("[SceneReturnButton] SceneLoader 연결 안 됨");
            return;
        }

        sceneLoader.MoveScene(targetSceneName);
    }
}

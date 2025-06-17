using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Fader sceneFader;

    private string nextSceneName;

    private Action onFadeInComplete;
    private Action onFadeOutComplete;

    private void OnEnable()
    {
        onFadeOutComplete += OnAwake;
        onFadeInComplete += OnFadeInComplete;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void MoveScene(string nextSceneName)
    {
        this.nextSceneName = nextSceneName;
        sceneFader.FadeIn(onFadeInComplete);
    }

    public void OnFadeInComplete()
    {
        if (LoadingScene(nextSceneName))                //로딩씬으로 이동   
        {
            SceneManager.LoadScene("LoadingScene");

        }
        else
        {
            SceneManager.LoadScene(nextSceneName);      //로딩이 필요 없는 씬이면 해당씬으로 바로 이동
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneFader.FadeOut(onFadeOutComplete); //씬이 로드되면 자동으로 페이드 아웃을 호출해준다.
    }

    public void OnAwake()
    {
        Time.timeScale = 1.0f; //씬이 로드되고 FadeOut이 완료되면 게임 시간을 흐르게 해준다.
    }

    public bool LoadingScene(string nextSceneName)     //로딩이 필요한 씬 구분
    {
        if (nextSceneName == "MainScene") return true;
        return false;
    }

    private void OnDestroy()
    {
        onFadeOutComplete -= OnAwake;
        onFadeInComplete -= OnFadeInComplete;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public (Fader, string) GetLoadingData()
    {
        return (sceneFader, nextSceneName);
    }
}

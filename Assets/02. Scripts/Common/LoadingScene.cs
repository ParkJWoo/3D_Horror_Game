using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private SceneLoader loader;

    private AsyncOperation asyncOperation;

    public string nextSceneName;

    public TextMeshProUGUI progressText;
    private bool loadEnd = false;

    private int progress;

    private bool fakeStart = false;
    private float fakeTime = 2f;
    private float interval = 0.1f;

    private Fader fader;

    private Action onComplete;

    private void Start()
    {
        loader = GameManager.Instance.sceneLoader;

        fader = loader.GetLoadingData().Item1;
        nextSceneName = loader.GetLoadingData().Item2;

        onComplete = LoadComplete;

        Invoke(nameof(Loading), fader.fadeInTime);
    }

    public void LoadSceneInfo(string _nextSceneName)
    {
        nextSceneName = _nextSceneName;
    }

    public void Loading()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        loadEnd = false;

        //로딩 진행 상황을 자연스럽게 연출하게 위해 ProgressWrite 와 yield return null 을 여러번 사용하여 진행
        asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
        asyncOperation.allowSceneActivation = false;

        ProgressWrite();

        yield return null;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress < 0.9f)
            {
                ProgressWrite();
            }
            else
            {
                if (progress < 100)
                {
                    if (!fakeStart)
                    {
                        fakeStart = true;
                        StartCoroutine(FakeProgress());
                    }
                }
                else
                {
                    if (!loadEnd)
                    {
                        loadEnd = true;

                        fader.FadeIn(onComplete); //fadeIn 호출
                    }
                }
            }
            yield return null;
        }
    }

    private IEnumerator FakeProgress()
    {
        float progressEnd = 0;
        float progressCount = fakeTime / interval;                              //FakeTime내에 남은 로딩을 채우기 위한 횟수. fakeTime(2), 간격(interval 0.1)일때 2초동안 0.1초 간격으로 로딩 진행시 20번이 필요하다. 2/0.1 = 20
        float progressAmount = interval / progressCount;                        //20번의 횟수가 1이 되려면 간격/횟수로 양을 구할 수 있다. 0.1/20 = 0.005
        yield return null;

        while (progress < 100)
        {                                                                       //남은 0.1의 프로그래스를 채워준다. 0.005 * 20 = 0.1 
            progressEnd += progressAmount;                                      //progress는 0~1까지. 0.9에서 나머지 0.1을 채우려면 0.005씩 채워주면된다. 0.005*20 = 0.1
            progress = (int)((asyncOperation.progress + progressEnd) * 100);
            progress = Mathf.Clamp(progress, 0, 100);                           //로딩 진행 상황이 100%를 넘어갈수 없으므로 100까지 묶어서 표시
            progressText.text = $"{progress}%";
            yield return new WaitForSecondsRealtime(interval);                  //로딩은 타임 스케일에 영향을 받지 않아야 함으로 realtime으로 처리한다.
        }
    }

    public void ProgressWrite()
    {
        if (progressText == null) return;

        progress = (int)(asyncOperation.progress * 100);                        //로딩에서 소수점 표시를 하지 않기위해 int로 처리
        progressText.text = $"{progress}%";
    }

    public void LoadComplete()  //현재 스크립트에서는 로딩이 완료되고 페이드가 끝나면 action함수를 통해 씬전환을 호출하는 구조이다.
    {
        if (!asyncOperation.allowSceneActivation)
        {
            asyncOperation.allowSceneActivation = true;
        }
    }
}

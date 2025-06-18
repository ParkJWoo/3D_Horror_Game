using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isNewGame;
    public PlaySceneManager playSceneManager;

    public SceneLoader sceneLoader;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        sceneLoader ??= GetComponentInChildren<SceneLoader>();
        if (sceneLoader == null) { Debug.LogError("SceneLoader를 찾지 못했습니다"); }
        SceneManager.sceneLoaded += Init;
        SceneManager.sceneUnloaded += UnLoad;
    }

    //private void Update()
    //{
    //    //SceneMoveTestCode();
    //}

    public void SceneMoveTestCode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            sceneLoader.MoveScene("Item");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            sceneLoader.MoveScene("MainScene");
        }
    }

    public void Init(Scene scene, LoadSceneMode mode)
    {
        playSceneManager = FindObjectOfType<PlaySceneManager>();
        if (playSceneManager == null)
        {
            Debug.Log("playSceneManager를 찾지 못했습니다");
        }
        else
        { 
            playSceneManager?.Init(); 
        }
    }

    public void UnLoad(Scene scene)
    {
        if (playSceneManager != null)
        {
            playSceneManager = null;
        }
    }
}

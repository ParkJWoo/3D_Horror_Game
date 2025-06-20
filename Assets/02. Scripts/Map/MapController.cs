using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] MapSO[] MapDatas = new MapSO[4];
    MapSO currentMapData;
    public int currentdataindex;
    [SerializeField] private GameObject MapLightContianor;
    List<Light> Lights = new List<Light>();
    List<float> Targetintenses = new List<float>();
    List<float> LerpSpeeds = new List<float>();
    float minLerpSpeed = 2f;
    float maxLerpSpeed = 7f;


    public Enemy slendermanEnemy;
    public float baseSlendermanSpeed = 3.5f;
    public float speedPerStage = 1.0f;

    public List<WhisperPuzzlePhoto> whisperPuzzlePhotos = new List<WhisperPuzzlePhoto>();

    public List<Door> doors = new List<Door>();

    private SaveManager saveManager;

    void Start()
    {
        saveManager = SaveManager.Instance;
        Init();
        GetAllLights();
    }

    private void Update()
    {
        for (int i = 0; i < Lights.Count; i++)
        {
            Lights[i].intensity = Mathf.Lerp(Lights[i].intensity, Targetintenses[i], Time.deltaTime * LerpSpeeds[i]);
        }
    }

    private void Init() // 요기 이제 저장된 맵정보를 불러오거나 없으면 처음맵정보 적용
    {
        if (GameManager.Instance.isNewGame) 
        {
            currentMapData = MapDatas[0];
            currentdataindex = 0;
        }
        else
        {
            SaveData saveData = saveManager.saveData;
            currentdataindex = saveData.lastCheckpoint;
            currentMapData = MapDatas[currentdataindex];

            for (int i = 0; i < whisperPuzzlePhotos.Count; i++)
            {
                whisperPuzzlePhotos[i].iskeyphoto = saveData.clearPhoto[i];
            }

            for (int i = 0; i < doors.Count; i++)
            {
                if (saveManager.saveData.openDoor[i]) 
                {
                    doors[i].OpenDoor();
                }
                else
                {
                    doors[i].CloseDoor();
                }
            }
        }

        //  최초 시작 시 슬랜더맨 속도 세팅
        if (slendermanEnemy == null)
        {
            Debug.LogWarning("[MapController] slendermanEnemy가 에디터에 할당되어 있지 않습니다!");
            return;
        }
        if (slendermanEnemy.Agent == null)
        {
            Debug.LogWarning("[MapController] slendermanEnemy의 NavMeshAgent 컴포넌트가 없습니다!");
            return;
        }

        float speed = baseSlendermanSpeed + currentdataindex * speedPerStage;
        slendermanEnemy.Agent.speed = speed;
    }

    private void GetAllLights() // 모든 광원 정보를 리스트에 저장
    {
        MapLightContianor.GetComponentsInChildren(Lights);
        for(int i = 0; i < Lights.Count; i++)
        {
            Lights[i].color = currentMapData.Lightinfo.Lightcolor;
            Targetintenses.Add(Lights[i].intensity);
            LerpSpeeds.Add(Random.Range(minLerpSpeed, maxLerpSpeed));
            StartCoroutine(Lightcorutine(i));
        }
    }

    public void GoNextStage()
    {
        currentdataindex++;
        if (currentdataindex >= MapDatas.Length)
        {
            // 엔딩
            return;
        }

        currentMapData = MapDatas[currentdataindex];

        // 슬랜더맨 속도 갱신
        if (slendermanEnemy != null && slendermanEnemy.Agent != null)
        {
            float speed = baseSlendermanSpeed + currentdataindex * speedPerStage;
            slendermanEnemy.Agent.speed = speed;
        }

        StopAllCoroutines();
        ResetLight();
        GetAllLights();
    }

    private void ResetLight()
    {
        Lights = new List<Light>();
        Targetintenses = new List<float>();
        LerpSpeeds = new List<float>();
    }

    IEnumerator Lightcorutine(int index) // 각 광원의 랜덤성을 설정
    {
        while(true)
        {
            Targetintenses[index] = Random.Range(currentMapData.Lightinfo.MinIntense, currentMapData.Lightinfo.MaxIntense);
            LerpSpeeds[index] = Random.Range(minLerpSpeed, maxLerpSpeed);
            float waittime = Random.Range(currentMapData.Lightinfo.MinInterval, currentMapData.Lightinfo.MaxInterval);
            yield return new WaitForSeconds(waittime);
        }
    }

    public void UpdateLastCheckpoint(int photoNum)
    {
        saveManager.UpdatePlayerData(CharacterManager.Instance.Player);
        PlaySceneManager.instance.itemManager.Save();
        saveManager.saveData.lastCheckpoint = currentdataindex;

        List<bool> photoSave = new List<bool>();
        for (int i = 0; i < whisperPuzzlePhotos.Count; i++)
        {
            photoSave.Add(whisperPuzzlePhotos[i].iskeyphoto);
        }
        saveManager.saveData.clearPhoto = photoSave;

        List<bool> doorSave = new List<bool>();
        for (int i = 0; i < doors.Count; i++)
        {
            doorSave.Add(doors[i].IsOpen);
        }
        saveManager.saveData.openDoor = doorSave;

        saveManager.SaveGame();
    }
}

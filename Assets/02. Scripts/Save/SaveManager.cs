using UnityEngine;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    public SaveData saveData = new SaveData();
    public OptionData optionData = new OptionData();

    private string gameDataPath;
    public string optionDataPath;
    private string gameDataFileName = "/save.json";
    private string optionDataFileName = "/option.json";
    private string keyWord = "dlka3o33kl12daah*%(* UHOi==";

    private void Awake()
    {
        gameDataPath = Application.persistentDataPath + gameDataFileName;
        optionDataPath = Application.persistentDataPath + optionDataFileName;
        Debug.Log($"{gameDataPath}\n{optionDataPath}");

        LoadGame();
        LoadOption();
    }


    public void SaveGame()
    {
        string data = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(gameDataPath, data);
        Debug.Log("저장완료");
    }

    public void SaveOption()
    {
        string data = JsonUtility.ToJson(optionData, true);
        File.WriteAllText(optionDataPath, data);
    }


    public void LoadGame()
    {
        if (GameManager.Instance.isNewGame)
        {
            TextAsset defaulData = Resources.Load<TextAsset>("Default");

            if (defaulData != null)
            {
                string loadData = defaulData.text;
                saveData = JsonUtility.FromJson<SaveData>(loadData);
                Debug.Log($"디폴트 데이터 로드 : {saveData}");
            }
            else
            {
                Debug.LogError($"디폴트 데이터 로드에 실패 했습니다.");
            }
        }
        else
        {
            string data = File.ReadAllText(gameDataPath);
            saveData = JsonUtility.FromJson<SaveData>(data);
        }
    }

    public void LoadOption()
    {
        if (!File.Exists(optionDataPath))
        {
            TextAsset currentOptionData = Resources.Load<TextAsset>("Option");
            if (optionData != null)
            {
                string loadOption = currentOptionData.text;
                optionData = JsonUtility.FromJson<OptionData>(loadOption);
                Debug.Log($"옵션 데이터 로드 : {optionData}");
            }
            else
            {
                Debug.LogError($"옵션 데이터 로드에 실패 했습니다.");
            }
        }
        else
        {
            string data = File.ReadAllText(optionDataPath);
            optionData = JsonUtility.FromJson<OptionData>(data);

            Debug.Log($"로드된 BGM 볼륨: {optionData.currentBgmVolume}, 뮤트: {optionData.currentBgmMute}");
        }
    }


    public SaveData GetCurrentSaveData()
    {
        return saveData;
    }

    public OptionData GetSoundOptionData()
    {
        return optionData;
    }


    public void Respawn()
    {
    }

    public void UpdateCheckpoint(Checkpoint checkpoint)
    {
    }

    public void UpdateSoundSetting(float bgmVolume, bool bgmMute, float sfxVolume, bool sfxMute)
    {
        optionData.currentBgmVolume = bgmVolume;
        optionData.currentSfxVolume = sfxVolume;
        optionData.currentBgmMute = bgmMute;
        optionData.currentSfxMute = sfxMute;
    }

    public void SaveButton()
    {
        SaveOption();
    }

    public void UpdatePlayerPosition(Transform player)
    {
        saveData.playerPosition = player.position;
    }

    public void UpdateEnemyPosition(Transform enemy)
    {
        if (!enemy.gameObject.activeInHierarchy) return;
        saveData.enemyPosition = enemy.position;
    }

    private string EncryptAndDecrypt(string data)
    {
        string result = "";

        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ keyWord[i % keyWord.Length]);
        }

        return result;
    }
}
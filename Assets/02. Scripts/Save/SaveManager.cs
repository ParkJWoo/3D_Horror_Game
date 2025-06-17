using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

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
        if (!File.Exists(gameDataPath))
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
                Debug.Log($"옵션 데이터 로드 : {optionData.currentBgmVolume}");
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

    public void UpdateLastCheckpoint(int stageIndex)
    {
        saveData.lastCheckpoint = stageIndex;
    }
    

    public void UpdateEquipItems(Player player)
    {
        ItemInstance[] playerEquipItems = player.Equipment.equipItems;
        SaveItemData[] currentEquipItemData = new SaveItemData[playerEquipItems.Length];
        
        for (int i = 0; i < playerEquipItems.Length; i++)
        {
            currentEquipItemData[i] = new SaveItemData(playerEquipItems[i]);
        }

        saveData.equipItemData = currentEquipItemData;
    }
    

    public void UpdateItemList(Player player)
    {
        ItemInstance[] playerItems = player.Inventory.invenItems;
        List<SaveItemData> currentItemData = new List<SaveItemData>();

        for (int i = 0; i < playerItems.Length; i++)
        {
            currentItemData.Add(new SaveItemData(playerItems[i]));
        }
        
        saveData.haveItemData = currentItemData;
    }

    public void UpdatePlayerPosition(Player player)
    {
        saveData.playerPosition = player.transform.position;;
    }
    

    public void UpdatePlayerData(Player player)
    {
        UpdatePlayerPosition(player);
        UpdateItemList(player);
        UpdateEquipItems(player);
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
    
    public void UpdateSoundSetting(SoundUI sound)
    {
        optionData.currentBgmVolume = sound.BgmSlider.value;
        optionData.currentSfxVolume = sound.SfxSlider.value;
        optionData.currentBgmMute = sound.BgmToggle.isOn;
        optionData.currentSfxMute = sound.SfxToggle.isOn;
    }

    public void SaveButton()
    {
        SaveOption();
    }
}
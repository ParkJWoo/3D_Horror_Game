using UnityEngine;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    public SaveData saveData = new SaveData();

    private string path;
    private string fileName = "/save.json";
    private string keyWord = "dlka3o33kl12daah*%(* UHOi==";

    private void Awake()
    {
        path = Application.persistentDataPath + fileName;
        Debug.Log(path);
        LoadGame();
    }
    

    public void SaveGame()
    {
        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, data);
        Debug.Log("저장완료");
    }

    public void LoadGame()
    {
        if (!File.Exists(path))
        {
            SaveGame();
        }

        string data = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(data);

        Debug.Log($"로드된 BGM 볼륨: {saveData.currentBgmVolume}, 뮤트: {saveData.currentBgmMute}");
    }

    public SaveData GetCurrentSaveData()
    {
        return saveData;
    }


    public void Respawn()
    {
    }

    public void UpdateCheckpoint(Checkpoint checkpoint)
    {
    }

    public void UpdateSoundSetting(float bgmVolume, bool bgmMute, float sfxVolume, bool sfxMute)
    {
        saveData.currentBgmVolume = bgmVolume;
        saveData.currentSfxVolume = sfxVolume;
        saveData.currentBgmMute = bgmMute;
        saveData.currentSfxMute = sfxMute;
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
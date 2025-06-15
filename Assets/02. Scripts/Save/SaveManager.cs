using UnityEngine;
using System.IO;

public class SaveManager :Singleton<SaveManager>
{
    public SaveData saveData = new SaveData();

    private string path;
    private string fileName = "/save";
    private string keyWord = "dlka3o33kl12daah*%(* UHOi==";

    private void Start()
    {
        path = Application.persistentDataPath + fileName;
        Debug.Log(path);
        
        LoadGame();
    }
    
    public void SaveGame()
    {
        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, EncryptAndDecrypt(data));
    }

    public void LoadGame()
    {
        if (!File.Exists(path))
        {
            SaveGame();
        }
        
        string data = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(EncryptAndDecrypt(data));
    }
    

    public void Respawn()
    {
        
    }

    public void SaveCheckpoint(Checkpoint checkpoint)
    {
        
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

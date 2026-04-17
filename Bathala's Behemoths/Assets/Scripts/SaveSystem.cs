using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private static SaveData _saveData = new SaveData();

    public bool hasSave = false;

    [System.Serializable]
    public struct SaveData
    {
        public PlayerStatsSaveData playerStatsData;
        public QuestStateSaveData questStateData;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            hasSave = File.Exists(SaveFileName());
        }
    }

    public string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }

    public void Save()
    {
        hasSave = true;
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    private void HandleSaveData()
    {
        PlayerStats.Instance.Save(ref _saveData.playerStatsData);
        QuestState.Instance.Save(ref _saveData.questStateData);
    }

    public void Load()
    {
        if (!File.Exists(SaveFileName()))
        {
            return;
        }

        string saveContent = File.ReadAllText(SaveFileName());

        _saveData = JsonUtility.FromJson<SaveData>(saveContent);
        HandleLoadData();
    }

    private void HandleLoadData()
    {
        PlayerStats.Instance.Load(_saveData.playerStatsData);
        QuestState.Instance.Load(_saveData.questStateData);
    }
}

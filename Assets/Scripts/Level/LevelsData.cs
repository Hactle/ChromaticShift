using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class LevelScene
{
    public string SceneName;

    public int LevelID;

    public bool IsUnlocked;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelsData : ScriptableObject
{
    public int CurrentLevelID;

    public List<LevelScene> levels = new();

    public void SetCurrentLevelID(int levelID)
    {
        CurrentLevelID = levelID;
    }

    public LevelScene GetLevel(int levelNumber)
    {
        return levels.Find(level => level.LevelID == levelNumber);
    }

    public LevelScene GetNextLevel(LevelScene currentLevel)
    {
        int nextLevelNumber = currentLevel.LevelID + 1;
        return GetLevel(nextLevelNumber);
    }

    public void UnlockNextLevel()
    {
        LevelScene level = GetLevel(CurrentLevelID + 1);
        if (level != null)
        {
            level.IsUnlocked = true;
        }
    }

    #region Save/Load
    private void OnApplicationQuit()
    {
        SaveData(levels);
    }

    public void SaveData(List<LevelScene> levels)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            int result = levels[i].IsUnlocked ? 1 : 0;

            PlayerPrefs.SetInt($"Level_{i}", result);
        }

        PlayerPrefs.Save();
    }

    public void LoadData(List<LevelScene> levels)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            int unlocked = PlayerPrefs.GetInt($"Level_{i}", 0);
            levels[i].IsUnlocked = (unlocked == 1);
        }
    }
    #endregion

    private void OnEnable()
    {
        LoadData(levels);

        Application.quitting += OnApplicationQuit;

        LevelCompletePoint.OnLevelComplete += UnlockNextLevel;
    }

    private void OnDisable()
    {
        LevelCompletePoint.OnLevelComplete -= UnlockNextLevel;
    }
}

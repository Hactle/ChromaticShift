using System.Collections.Generic;
using UnityEngine;

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

    private void OnEnable()
    {
        LevelCompletePoint.OnLevelComplete += UnlockNextLevel;
    }

    private void OnDisable()
    {
        LevelCompletePoint.OnLevelComplete -= UnlockNextLevel;
    }
}

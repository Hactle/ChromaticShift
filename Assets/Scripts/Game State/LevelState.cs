using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelState : MonoBehaviour
{
    [SerializeField] private LevelsData _levelData;

    public bool IsPaused { get; private set; }

    public void LoadNextLevel()
    {   
        _levelData.SetCurrentLevelID(_levelData.CurrentLevelID + 1);
        StartCoroutine(LoadSceneCoroutine(_levelData.CurrentLevelID)); 
        ResumeGame();
    }

    private IEnumerator LoadSceneCoroutine(int levelID)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelID);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Прогресс загрузки: " + (progress * 100) + "%");
            yield return null;
        }
    }

    public void PauseGamee()
    {
        Time.timeScale = 0f;
        IsPaused = true;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        AudioListener.pause = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(_levelData.CurrentLevelID);
        ResumeGame();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        ResumeGame();
    }
}

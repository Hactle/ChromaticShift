using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelsData _levelData;

    private void Start()
    {
        _levelData.SetCurrentLevelID(0);
    }

    public void LoadLevel(int levelID)
    {   
        _levelData.SetCurrentLevelID(levelID);
        StartCoroutine(LoadSceneCoroutine(_levelData.GetLevel(levelID).SceneName));     
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Прогресс загрузки: " + (progress * 100) + "%");
            yield return null;
        }
    }

    public void QuitGame() 
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

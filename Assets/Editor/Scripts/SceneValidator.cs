using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


[InitializeOnLoad]
public class SceneValidator
{
    static SceneValidator()
    {
        EditorSceneManager.sceneSaved += OnSceneSaved;
    }

    private static void OnSceneSaved(Scene scene)
    {
        ValidateSpawnPoints(scene);
    }

    [MenuItem("Tools/Validate Current Scene")]
    public static void ValidateCurrentScene()
    {
        ValidateSpawnPoints(EditorSceneManager.GetActiveScene());
    }

    private static void ValidateSpawnPoints(Scene scene)
    {
        if (!IsLevelScene(scene))
        {
            return;
        }

        GameObject[] allObjects = scene.GetRootGameObjects();
        bool spawnPointFound = false;

        foreach (var go in allObjects)
        {
            if (go.CompareTag("SpawnPoint"))
            {
                spawnPointFound = true;
                break;
            }
        }
        if (!spawnPointFound)
        {
            EditorUtility.DisplayDialog("Validation Scene",
                $"In Scene '{scene.name}' was not found player spawn (PlayerSpawnPoint)!",
                "Œ ");
            Debug.LogError($"SCENE VALIDATION FAILED: in scene '{scene.name}' was not found player spawn!");
        }
    }

    private static bool IsLevelScene(Scene scene)
    {
        string sceneName = scene.name.ToLower();
        string scenePath = scene.path.ToLower();

        if (sceneName.Contains("level") || sceneName.Contains("lvl") || sceneName.Contains("stage"))
        {
            return true;
        }

        if (scenePath.Contains("/levels/") || scenePath.Contains("/game/"))
        {
            return true;
        }

        return false;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeDetector : MonoBehaviour
{
    public static string CurrentSceneName;
    
    private void Start()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or unwanted calls
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }
    
    private static void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        CurrentSceneName = newScene.name;
        Debug.Log("iXRLib - Active scene changed to " + newScene.name);
        iXR.Event("Scene Changed", $"Scene Name={newScene.name}");
    }
}
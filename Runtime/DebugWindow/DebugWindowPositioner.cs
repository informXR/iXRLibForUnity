using UnityEngine;

public class DebugWindowPositioner : MonoBehaviour
{
    private static DebugWindowPositioner _instance;
    
    public static void Initialize()
    {
        if (_instance != null) return;
        
        var singletonObject = new GameObject("DebugPositioner");
        _instance = singletonObject.AddComponent<DebugWindowPositioner>();
        DontDestroyOnLoad(singletonObject);
    }
    
    private void Start()
    {
        if (!Configuration.instance.debugWindow) return;
        
        GameObject debugDisplay = Resources.Load<GameObject>("Prefabs/iXRDebugWindow");
        if (debugDisplay != null)
        {
            Instantiate(debugDisplay, Camera.main.transform);
        }
        else
        {
            Debug.LogError("Failed to load debug display prefab");
        }
    }
}
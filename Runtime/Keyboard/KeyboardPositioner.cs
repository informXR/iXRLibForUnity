using UnityEngine;

public class KeyboardPositioner : MonoBehaviour
{
    private GameObject _keyboard;
    private static KeyboardPositioner _instance;
    private string _typedString;
    
    public static void Initialize()
    {
        if (_instance != null) return;
        
        var singletonObject = new GameObject("KeyboardPositioner");
        _instance = singletonObject.AddComponent<KeyboardPositioner>();
        DontDestroyOnLoad(singletonObject);
    }
    
    private void Start()
    {
        _keyboard = Resources.Load<GameObject>("Prefabs/iXRKeyboard");
        if (_keyboard != null)
        {
            Instantiate(_keyboard, Camera.main.transform);
        }
        else
        {
            Debug.LogError("Failed to load keyboard prefab");
        }
    }
}
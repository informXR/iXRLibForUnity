using UnityEngine;
using iXRLib;

public class ShutDown : MonoBehaviour
{
    private static ShutDown _instance;
    
    public static void Initialize()
    {
        if (_instance != null) return;
        
        var singletonObject = new GameObject("ShutDown");
        _instance = singletonObject.AddComponent<ShutDown>();
        DontDestroyOnLoad(singletonObject);
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Debug.Log("iXRLib - Application lost focus");
            iXRInit.End();
        }
    }
}
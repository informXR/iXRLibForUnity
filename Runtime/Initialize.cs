using System.Runtime.InteropServices;
using iXRLib;
using UnityEngine;

public class Initialize : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void LoadIxrLib();
#endif
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        LoadIxrLib();
#endif
        var bootstrapGo = new GameObject("Bootstrap");
        bootstrapGo.AddComponent<Bootstrap>();
        DontDestroyOnLoad(bootstrapGo);

        iXRInit.Start();
    }
}
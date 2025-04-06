using System;
using UnityEngine;

public static class Initialize
{
    public static readonly long StartTimeMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
        //iXRInit.Start();
#if UNITY_ANDROID
        ObjectAttacher.Attach<ExceptionLogger>("ExceptionLogger");
        ObjectAttacher.Attach<DeviceModel>("DeviceModel");
#endif
        ObjectAttacher.Attach<KeyboardHandler>("KeyboardHandler"); // Needs to come before Auth in case auth needs keyboard
        ObjectAttacher.Attach<Authentication>("Authentication");
        ObjectAttacher.Attach<TrackSystemInfo>("TrackSystemInfo");
        ObjectAttacher.Attach<ExitPollHandler>("ExitPollHandler");
        ObjectAttacher.Attach<SceneChangeDetector>("SceneChangeDetector");
        ObjectAttacher.Attach<DataBatcher>("DataBatcher");
#if UNITY_ANDROID
        if (Configuration.Instance.headsetTracking)
        {
            ObjectAttacher.Attach<TrackInputDevices>("TrackInputDevices");
        }
#endif
    }
}

public class ObjectAttacher : MonoBehaviour
{
    public static T Attach<T>(string name) where T : MonoBehaviour
    {
        var go = new GameObject(name);
        DontDestroyOnLoad(go);
        return go.AddComponent<T>();
    }
}
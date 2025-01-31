using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using iXRLib;
using UnityEngine;

public static class Initialize
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
        //TestDiagnosticStringCallbackMechanism();
        iXRInit.Start();
        SetConfigValues();
        ObjectAttacher.Attach<ExceptionLogger>("ExceptionLogger");
#if UNITY_ANDROID
        ObjectAttacher.Attach<DeviceModel>("DeviceModel");
#endif
        ObjectAttacher.Attach<KeyboardHandler>("KeyboardHandler"); // Needs to come before Auth in case auth needs keyboard
        ObjectAttacher.Attach<Authentication>("Authentication");
        ObjectAttacher.Attach<TrackSystemInfo>("TrackSystemInfo");
        ObjectAttacher.Attach<ExitPollHandler>("ExitPollHandler");
#if UNITY_ANDROID
        if (Configuration.Instance.headsetTracking)
        {
            ObjectAttacher.Attach<TrackInputDevices>("TrackInputDevices");
        }
#endif
    }

    private static void SetConfigValues()
    {
        iXRLib.Configuration.restUrl = Configuration.Instance.restUrl;
        iXRLib.Configuration.sendRetriesOnFailure = (uint)Configuration.Instance.sendRetriesOnFailure;
        iXRLib.Configuration.sendRetryInterval = TimeSpan.FromSeconds(Configuration.Instance.sendRetryIntervalSeconds);
        iXRLib.Configuration.sendNextBatchWait = TimeSpan.FromSeconds(Configuration.Instance.sendNextBatchWaitSeconds);
        iXRLib.Configuration.stragglerTimeout = TimeSpan.FromSeconds(Configuration.Instance.stragglerTimeoutSeconds);
        iXRLib.Configuration.eventsPerSendAttempt = (uint)Configuration.Instance.eventsPerSendAttempt;
        iXRLib.Configuration.logsPerSendAttempt = (uint)Configuration.Instance.logsPerSendAttempt;
        iXRLib.Configuration.telemetryEntriesPerSendAttempt = (uint)Configuration.Instance.telemetryEntriesPerSendAttempt;
        iXRLib.Configuration.storageEntriesPerSendAttempt = (uint)Configuration.Instance.storageEntriesPerSendAttempt;
        iXRLib.Configuration.pruneSentItemsOlderThan = TimeSpan.FromHours(Configuration.Instance.pruneSentItemsOlderThanHours);
        iXRLib.Configuration.maximumCachedItems = (uint)Configuration.Instance.maximumCachedItems;
        iXRLib.Configuration.retainLocalAfterSent = Configuration.Instance.retainLocalAfterSent;
    }

    private static void TestDiagnosticStringCallbackMechanism()
    {
        iXRLibAnalytics.SetDiagnosticStringCallback(DiagnosticString);
        iXRLibAnalytics.TestDiagnosticStringCallbackMechanism();
    }

    private static async Task DiagnosticString(string szString)
    {
        await Task.Run(() => Debug.Log($"iXRLib - {szString}"));
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
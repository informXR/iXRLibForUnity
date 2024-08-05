using UnityEngine;

public class Configuration : ScriptableObject
{
    private static Configuration _instance;
    public static Configuration instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            _instance = Resources.Load<Configuration>("informXR");
            if (_instance == null)
            {
                _instance = CreateInstance<Configuration>();
            }
            
            return _instance;
        }
    }
    
    [Tooltip("Required")] public string appID;
    [Tooltip("Optional")] public string orgID;
    [Tooltip("Optional")] public string authSecret;
    
    public bool headsetTracking;
    public int trackingUpdatesPerMinute = 4;
    
    public string restUrl = "https://libapi.informxr.io/";

    public int sendRetriesOnFailure = 3;
    public int sendRetryIntervalSeconds = 3;
    public int sendNextBatchWaitSeconds = 30;
    public int stragglerTimeoutSeconds = 15;
    public int eventsPerSendAttempt = 4;
    public int logsPerSendAttempt = 4;
    public int telemetryEntriesPerSendAttempt = 4;
    public int storageEntriesPerSendAttempt = 4;
    public int pruneSentItemsOlderThanHours = 12;
    public int maximumCachedItems = 1024;
    public bool retainLocalAfterSent;
}
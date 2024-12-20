using UnityEngine;

public interface IConfigurationService
{
    Configuration GetConfiguration();
}

public class ConfigurationService : IConfigurationService
{
    private Configuration _instance;
    
    public Configuration GetConfiguration()
    {
        if (_instance != null) return _instance;
            
        _instance = Resources.Load<Configuration>("informXR");
        if (_instance == null)
        {
            _instance = ScriptableObject.CreateInstance<Configuration>();
        }
            
        return _instance;
    }
}

public class Configuration : ScriptableObject
{
    [Tooltip("Required")] public string appID;
    [Tooltip("Optional")] public string orgID;
    [Tooltip("Optional")] public string authSecret;
    
    public bool headsetTracking;
    public int trackingUpdatesPerMinute = 4;
    
    public string restUrl = "https://libapi.informxr.io/v1/";

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
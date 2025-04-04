using System;
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

    public void SetConfigFromPayload(Authentication.ConfigPayload payload)
    {
        if (!string.IsNullOrEmpty(payload.restUrl)) restUrl = payload.restUrl;
        if (!string.IsNullOrEmpty(payload.sendRetriesOnFailure)) sendRetriesOnFailure = Convert.ToInt32(payload.sendRetriesOnFailure);
        if (!string.IsNullOrEmpty(payload.sendRetryInterval)) sendRetryIntervalSeconds = Convert.ToInt32(payload.sendRetryInterval);
        if (!string.IsNullOrEmpty(payload.sendNextBatchWait)) sendNextBatchWaitSeconds = Convert.ToInt32(payload.sendNextBatchWait);
        if (!string.IsNullOrEmpty(payload.stragglerTimeout)) stragglerTimeoutSeconds = Convert.ToInt32(payload.stragglerTimeout);
        if (!string.IsNullOrEmpty(payload.eventsPerSendAttempt)) eventsPerSendAttempt = Convert.ToInt32(payload.eventsPerSendAttempt);
        if (!string.IsNullOrEmpty(payload.logsPerSendAttempt)) logsPerSendAttempt = Convert.ToInt32(payload.logsPerSendAttempt);
        if (!string.IsNullOrEmpty(payload.telemetryEntriesPerSendAttempt)) telemetryEntriesPerSendAttempt = Convert.ToInt32(payload.telemetryEntriesPerSendAttempt);
        if (!string.IsNullOrEmpty(payload.storageEntriesPerSendAttempt)) storageEntriesPerSendAttempt = Convert.ToInt32(payload.storageEntriesPerSendAttempt);
        if (!string.IsNullOrEmpty(payload.pruneSentItemsOlderThan)) pruneSentItemsOlderThanHours = Convert.ToInt32(payload.pruneSentItemsOlderThan);
        if (!string.IsNullOrEmpty(payload.maximumCachedItems)) maximumCachedItems = Convert.ToInt32(payload.maximumCachedItems);
        if (!string.IsNullOrEmpty(payload.retainLocalAfterSent)) retainLocalAfterSent = Convert.ToBoolean(payload.retainLocalAfterSent);
    }
}
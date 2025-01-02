using System;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private static Bootstrap _instance;
    private IInitializationService _initializationService;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeServices();
        SetConfigValues();
        _initializationService.InitializeAll();
    }

    private void InitializeServices()
    {
        // Create service instances
        var configService = new ConfigurationService();
        
        // Initialize MonoBehaviour services first
        //var exitPollHandler = CreateServiceObject<ExitPollHandler>("ExitPollHandler");
        var authService = CreateServiceObject<AuthenticationService>("AuthenticationService");
        var trackInputDevices = CreateServiceObject<TrackInputDevices>("TrackInputDevices");
        var trackSystemInfo = CreateServiceObject<TrackSystemInfo>("TrackSystemInfo");
        
        // Create IxrService with dependency
        var ixrService = new IxrService();
        
        // Register services with ServiceLocator
        ServiceLocator.RegisterService<IConfigurationService>(configService);
        ServiceLocator.RegisterService<IIxrService>(ixrService);
        ServiceLocator.RegisterService<IAuthenticationService>(authService);
        ServiceLocator.RegisterService<ITrackInputDevices>(trackInputDevices);
        //ServiceLocator.RegisterService<IExitPollHandler>(exitPollHandler);
        ServiceLocator.RegisterService<ITrackSystemInfo>(trackSystemInfo);

        // Create initialization service
        _initializationService = new InitializationService(
            configService, 
            authService, 
            trackInputDevices, 
            //exitPollHandler,
            ixrService,
            trackSystemInfo);
        ServiceLocator.RegisterService(_initializationService);
    }

    private void SetConfigValues()
    {
        var config = ServiceLocator.GetService<IConfigurationService>().GetConfiguration();
        iXRLib.Configuration.restUrl = config.restUrl;
        iXRLib.Configuration.sendRetriesOnFailure = (uint)config.sendRetriesOnFailure;
        iXRLib.Configuration.sendRetryInterval = TimeSpan.FromSeconds(config.sendRetryIntervalSeconds);
        iXRLib.Configuration.sendNextBatchWait = TimeSpan.FromSeconds(config.sendNextBatchWaitSeconds);
        iXRLib.Configuration.stragglerTimeout = TimeSpan.FromSeconds(config.stragglerTimeoutSeconds);
        iXRLib.Configuration.eventsPerSendAttempt = (uint)config.eventsPerSendAttempt;
        iXRLib.Configuration.logsPerSendAttempt = (uint)config.logsPerSendAttempt;
        iXRLib.Configuration.telemetryEntriesPerSendAttempt = (uint)config.telemetryEntriesPerSendAttempt;
        iXRLib.Configuration.storageEntriesPerSendAttempt = (uint)config.storageEntriesPerSendAttempt;
        iXRLib.Configuration.pruneSentItemsOlderThan = TimeSpan.FromHours(config.pruneSentItemsOlderThanHours);
        iXRLib.Configuration.maximumCachedItems = (uint)config.maximumCachedItems;
        iXRLib.Configuration.retainLocalAfterSent = config.retainLocalAfterSent;
    }

    private T CreateServiceObject<T>(string name) where T : MonoBehaviour
    {
        var go = new GameObject(name);
        DontDestroyOnLoad(go);
        return go.AddComponent<T>();
    }
} 
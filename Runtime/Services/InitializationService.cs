using iXRLib;
using UnityEngine;

public interface IInitializationService
{
    void InitializeAll();
}

public class InitializationService : IInitializationService
{
    private readonly IConfigurationService _configService;
    private readonly IAuthenticationService _authService;
    private readonly ITrackInputDevices _trackInputDevices;
    private readonly IExitPollHandler _exitPollHandler;
    private readonly IIxrService _ixrService;
    private readonly ITrackSystemInfo _trackSystemInfo;

    public InitializationService(
        IConfigurationService configService,
        IAuthenticationService authService,
        ITrackInputDevices trackInputDevices,
        IExitPollHandler exitPollHandler,
        IIxrService ixrService,
        ITrackSystemInfo trackSystemInfo)
    {
        _configService = configService;
        _authService = authService;
        _trackInputDevices = trackInputDevices;
        _exitPollHandler = exitPollHandler;
        _ixrService = ixrService;
        _trackSystemInfo = trackSystemInfo;
    }

    public void InitializeAll()
    {
        var config = _configService.GetConfiguration();
        SetConfigurationValues(config);
        
        KeyboardHandler.Initialize();
        _authService.Initialize(_configService, _ixrService);
        _trackSystemInfo.Initialize();
        _exitPollHandler.Initialize(_ixrService);

#if UNITY_ANDROID
        _trackInputDevices.Initialize(_ixrService);
#endif
    }

    private void SetConfigurationValues(Configuration config)
    {
        iXRLib.Configuration.restUrl = config.restUrl;
        iXRLib.Configuration.sendRetriesOnFailure = (uint)config.sendRetriesOnFailure;
        iXRLib.Configuration.sendRetryInterval = System.TimeSpan.FromSeconds(config.sendRetryIntervalSeconds);
        iXRLib.Configuration.sendNextBatchWait = System.TimeSpan.FromSeconds(config.sendNextBatchWaitSeconds);
        iXRLib.Configuration.stragglerTimeout = System.TimeSpan.FromSeconds(config.stragglerTimeoutSeconds);
        iXRLib.Configuration.eventsPerSendAttempt = (uint)config.eventsPerSendAttempt;
        iXRLib.Configuration.logsPerSendAttempt = (uint)config.logsPerSendAttempt;
        iXRLib.Configuration.telemetryEntriesPerSendAttempt = (uint)config.telemetryEntriesPerSendAttempt;
        iXRLib.Configuration.storageEntriesPerSendAttempt = (uint)config.storageEntriesPerSendAttempt;
        iXRLib.Configuration.pruneSentItemsOlderThan = System.TimeSpan.FromHours(config.pruneSentItemsOlderThanHours);
        iXRLib.Configuration.maximumCachedItems = (uint)config.maximumCachedItems;
        iXRLib.Configuration.retainLocalAfterSent = config.retainLocalAfterSent;
    }
} 
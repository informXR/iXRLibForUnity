using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[DefaultExecutionOrder(100)] // Doesn't matter when this one runs
public class TrackSystemInfo : MonoBehaviour
{
    private int _lastFrameCount;
    private float _lastTime;
    private const int FrameRateCheckIntervalSeconds = 10;
    private static TrackSystemInfo _instance;
    
    public static void Initialize()
    {
        if (_instance != null) return;
        
        var singletonObject = new GameObject("TrackSystemInfo");
        _instance = singletonObject.AddComponent<TrackSystemInfo>();
        DontDestroyOnLoad(singletonObject);
    }
    
    private void Start()
    {
        InvokeRepeating(nameof(CheckSystemInfo), 0, 60); // Call every 60 seconds
        InvokeRepeating(nameof(CheckFrameRate), 0, FrameRateCheckIntervalSeconds);
    }

    private void CheckSystemInfo()
    {
        var batteryData = new Dictionary<string, string>
        {
            ["Percentage"] = (int)(SystemInfo.batteryLevel * 100 + 0.5) + "%",
            ["Status"] = SystemInfo.batteryStatus.ToString()
        };
        iXRLog.TelemetryEntry("Battery", batteryData);
        
        var memoryData = new Dictionary<string, string>
        {
            ["Total Allocated"] = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong().ToString(),
            ["Total Reserved"] = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong().ToString(),
            ["Total Unused Reserved"] = UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong().ToString()
        };
        iXRLog.TelemetryEntry("Memory", memoryData);
    }
    
    private void CheckFrameRate()
    {
        float frameRate = (Time.frameCount - _lastFrameCount) / (Time.time - _lastTime);
        var telemetryData = new Dictionary<string, string>
        {
            ["Per Second"] = frameRate.ToString(CultureInfo.InvariantCulture)
        };
        iXRLog.TelemetryEntry("Frame Rate", telemetryData);
        _lastFrameCount = Time.frameCount;
        _lastTime = Time.time;
    }
}
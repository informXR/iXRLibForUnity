using System.Collections.Generic;
using System.Globalization;
using iXRLib;
using UnityEngine;

[DefaultExecutionOrder(100)] // Doesn't matter when this one runs
public class TrackSystemInfo : MonoBehaviour
{
    private int _lastFrameCount;
    private float _lastTime;
    private const int FrameRateCheckIntervalSeconds = 10;
    
    private void Start()
    {
        InvokeRepeating(nameof(CheckSystemInfo), 0, 60); // Call every 60 seconds
        InvokeRepeating(nameof(CheckFrameRate), 0, FrameRateCheckIntervalSeconds);
    }

    private void CheckSystemInfo()
    {
        iXRBase.CaptureTimeStamp();
        var batteryData = new Dictionary<string, string>
        {
            ["Percentage"] = (int)(SystemInfo.batteryLevel * 100 + 0.5) + "%",
            ["Status"] = SystemInfo.batteryStatus.ToString()
        };
        iXR.TelemetryEntry("Battery", batteryData);
        
        var memoryData = new Dictionary<string, string>
        {
            ["Total Allocated"] = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong().ToString(),
            ["Total Reserved"] = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong().ToString(),
            ["Total Unused Reserved"] = UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong().ToString()
        };
        iXR.TelemetryEntry("Memory", memoryData);
        iXRBase.UnCaptureTimeStamp();
    }
    
    private void CheckFrameRate()
    {
        float timeDiff = Time.time - _lastTime;
        if (timeDiff == 0) return;
        
        float frameRate = (Time.frameCount - _lastFrameCount) / timeDiff;
        var telemetryData = new Dictionary<string, string>
        {
            ["Per Second"] = frameRate.ToString(CultureInfo.InvariantCulture)
        };
        iXR.TelemetryEntry("Frame Rate", telemetryData);
        _lastFrameCount = Time.frameCount;
        _lastTime = Time.time;
    }
}
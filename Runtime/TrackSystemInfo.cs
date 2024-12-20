using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public interface ITrackSystemInfo
{
    void Initialize();
}

[DefaultExecutionOrder(100)] // Doesn't matter when this one runs
public class TrackSystemInfo : MonoBehaviour, ITrackSystemInfo
{
    private IIxrService _ixrService;
    private float _lastCpuUsage;
    private float _lastMemoryUsage;
    private int _lastFrameCount;
    private float _lastTime;
    private const int FrameRateCheckIntervalSeconds = 10;

    public void Initialize()
    {
        _ixrService = ServiceLocator.GetService<IIxrService>();
        InvokeRepeating(nameof(UpdateSystemInfo), 0, 60);
        InvokeRepeating(nameof(CheckFrameRate), 0, FrameRateCheckIntervalSeconds);
    }

    private void UpdateSystemInfo()
    {
        var cpuUsage = GetCPUUsage();
        if (Mathf.Abs(cpuUsage - _lastCpuUsage) > 0.01f)
        {
            var cpuDict = new Dictionary<string, string>
            {
                ["usage"] = cpuUsage.ToString(CultureInfo.InvariantCulture)
            };
            _ixrService.TelemetryEntry("CPU Usage", cpuDict);
            _lastCpuUsage = cpuUsage;
        }

        var memoryUsage = GetMemoryUsage();
        if (Mathf.Abs(memoryUsage - _lastMemoryUsage) > 0.01f)
        {
            var memoryDict = new Dictionary<string, string>
            {
                ["usage"] = memoryUsage.ToString(CultureInfo.InvariantCulture)
            };
            _ixrService.TelemetryEntry("Memory Usage", memoryDict);
            _lastMemoryUsage = memoryUsage;
        }

        var batteryDict = new Dictionary<string, string>
        {
            ["level"] = SystemInfo.batteryLevel.ToString(CultureInfo.InvariantCulture),
            ["status"] = SystemInfo.batteryStatus.ToString()
        };
        _ixrService.TelemetryEntry("Battery", batteryDict);
    }

    private static float GetCPUUsage()
    {
        return 0; // TODO: Implement CPU usage tracking
    }

    private static float GetMemoryUsage()
    {
        return UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / (float)UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong();
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
        _ixrService.TelemetryEntry("Frame Rate", telemetryData);
        _lastFrameCount = Time.frameCount;
        _lastTime = Time.time;
    }
}
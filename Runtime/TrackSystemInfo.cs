using iXRLib;
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
        iXRSend.AddTelemetryEntry("Battery", $"Percentage, {(int)(SystemInfo.batteryLevel * 100 + 0.5)}%");
        iXRSend.AddTelemetryEntry("Battery", $"Status, {SystemInfo.batteryStatus}");
        
        iXRSend.AddTelemetryEntry("Memory",
            $"Total Allocated, {UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong()}");
        iXRSend.AddTelemetryEntry("Memory",
            $"Total Reserved, {UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong()}");
        iXRSend.AddTelemetryEntry("Memory",
            $"Total Unused Reserved, {UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong()}");
    }
    
    private void CheckFrameRate()
    {
        float frameRate = (Time.frameCount - _lastFrameCount) / (Time.time - _lastTime);
        iXRSend.AddTelemetryEntry("Frame Rate", $"Per Second,{frameRate}");
        _lastFrameCount = Time.frameCount;
        _lastTime = Time.time;
    }
}
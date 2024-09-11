using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugDisplay : MonoBehaviour
{
    public TextMeshProUGUI logText;
    public int maxLines = 18;

    private readonly Queue<string> _logQueue = new Queue<string>();

    private void OnEnable()
    {
        // Subscribe to Unity's log callback
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string newLog = $"{Time.time} {type}: {logString}";
        _logQueue.Enqueue(newLog);
        if (_logQueue.Count > maxLines)
        {
            _logQueue.Dequeue();
        }
        
        logText.text = string.Join("\n", _logQueue.ToArray());
    }
}
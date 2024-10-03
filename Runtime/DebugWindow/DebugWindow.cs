using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugWindow : MonoBehaviour
{
    public TMP_Text logText;
    public ScrollRect scrollRect;
    
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
        string msgText = logText.text;
        if (!string.IsNullOrEmpty(msgText)) msgText += "\n";
        
        float roundedTime = Mathf.Round(Time.time * 100f) / 100f;
        string newLog = $"[{roundedTime}] {type}: {logString}";
        msgText += newLog;
        logText.text = msgText;
        
        // Force the layout to update immediately
        LayoutRebuilder.ForceRebuildLayoutImmediate(logText.rectTransform);

        // Scroll to the bottom to show the latest text
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}

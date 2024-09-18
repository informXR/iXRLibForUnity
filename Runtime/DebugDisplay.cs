using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugDisplay : MonoBehaviour
{
    public TextMeshProUGUI logText;
    private const int MaxLines = 15;

    private readonly Queue<string> _logQueue = new Queue<string>();
    
    public Canvas canvasToToggle;
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        // Instantiate the input actions
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // Subscribe to Unity's log callback
        Application.logMessageReceived += HandleLog;
        
        // Enable the action map and listen for the button press
        _playerInputActions.Enable();
        _playerInputActions.Newactionmap.ToggleDebugWindow.performed += ToggleCanvas;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        
        _playerInputActions.Newactionmap.ToggleDebugWindow.performed -= ToggleCanvas;
        _playerInputActions.Disable();
    }
    
    private void ToggleCanvas(InputAction.CallbackContext context)
    {
        // Toggle the Canvas visibility
        canvasToToggle.enabled = !canvasToToggle.enabled;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string newLog = $"{Time.time} {type}: {logString}";
        _logQueue.Enqueue(newLog);
        if (_logQueue.Count > MaxLines)
        {
            _logQueue.Dequeue();
        }
        
        logText.text = string.Join("\n", _logQueue.ToArray());
    }
}
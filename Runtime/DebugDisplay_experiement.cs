using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class DebugDisplay_experiment : MonoBehaviour
{
    public TextMeshProUGUI logText;
    private const int MaxLines = 15;

    private readonly Queue<string> _logQueue = new Queue<string>();
    
    public Canvas canvasToToggle;
    private PlayerInputActions _playerInputActions;

    [SerializeField] private Vector3 offsetFromController = new Vector3(0, 0.1f, 0.05f);
    [SerializeField] private Vector3 rotationOffset = new Vector3(30, 0, 0);

    // Removed _isAttached field
    [SerializeField] private string _attachedTo = "None";

    private void Awake()
    {
        // Instantiate the input actions
        _playerInputActions = new PlayerInputActions();

        // Check if debug display is enabled in the config
        if (!IsDebugDisplayEnabled())
        {
            gameObject.SetActive(false);
            return;
        }

        AttachToController();
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

    private bool IsDebugDisplayEnabled()
    {
        return Configuration.instance.debugDisplay;
    }    

    private void AttachToController()
    {
        var targetController = Configuration.instance.debugDisplaySide == Configuration.DebugDisplaySide.Left
            ? FindLeftHandController()
            : FindRightHandController();

        if (targetController != null)
        {
            Debug.Log($"{Configuration.instance.debugDisplaySide} hand controller found. Attaching DebugDisplay.");
            transform.SetParent(targetController, false);
            transform.localPosition = offsetFromController;
            transform.localRotation = Quaternion.Euler(rotationOffset);
            Debug.Log($"DebugDisplay attached. Local position: {transform.localPosition}, Local rotation: {transform.localRotation.eulerAngles}");
            
            // Removed _isAttached = true;
            _attachedTo = Configuration.instance.debugDisplaySide.ToString();
        }
        else
        {
            Debug.LogWarning($"{Configuration.instance.debugDisplaySide} hand controller not found. DebugDisplay will not be attached.");
            
            // Removed _isAttached = false;
            _attachedTo = "None";
        }
    }

    private Transform FindLeftHandController()
    {
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, devices);
        
        if (devices.Count > 0)
        {
            // Assuming the first device in the list is the left controller
            // Since we can't directly get the transform, we'll return the main camera's transform as a fallback
            return Camera.main.transform;
        }
        
        return null;
    }

    private Transform FindRightHandController()
    {
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, devices);
        
        if (devices.Count > 0)
        {
            Debug.Log($"Found {devices.Count} right hand controller(s).");
            return Camera.main.transform;
        }
        
        Debug.LogWarning("No right hand controllers found.");
        return null;
    }
}
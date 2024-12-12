using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EyeTracking : MonoBehaviour
{
    private static EyeTracking _instance;
    private const int CheckIntervalSeconds = 1;
    private InputDevice _eyeTrackingDevice;
    
    public static void Initialize()
    {
        if (_instance != null) return;
        
        var singletonObject = new GameObject("EyeTracking");
        _instance = singletonObject.AddComponent<EyeTracking>();
        DontDestroyOnLoad(singletonObject);
    }
    
    private void Start()
    {
        // Find the eye tracking device
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.EyeTracking, devices);

        if (devices.Count > 0)
        {
            _eyeTrackingDevice = devices[0];
            Debug.Log($"iXRLib - Eye Tracking Device Found: {_eyeTrackingDevice.name}");
        }
        else
        {
            Debug.LogError("iXRLib - No Eye Tracking Device Found.");
        }
        
        InvokeRepeating(nameof(GetLookDirection), 0, CheckIntervalSeconds);
    }
    
    private Vector3 GetLookDirection()
    {
        if (_eyeTrackingDevice.isValid)
        {
            // Try to get the gaze position (position of the eye's gaze in world space)
            if (_eyeTrackingDevice.TryGetFeatureValue(new InputFeatureUsage<Vector3>("EyeGazePosition"), out Vector3 gazePosition))
            {
                Debug.Log($"iXRLib - Gaze Position: {gazePosition}");
            }

            // Try to get the gaze rotation (direction of the gaze)
            if (_eyeTrackingDevice.TryGetFeatureValue(new InputFeatureUsage<Quaternion>("EyeGazeRotation"), out Quaternion gazeRotation))
            {
                Debug.Log($"iXRLib - Gaze Rotation: {gazeRotation}");
            }
        }

        return new Vector3();
        
        var centerEye = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);

        if (!centerEye.TryGetFeatureValue(CommonUsages.eyesData, out Eyes eyes)) return Camera.main.transform.forward;
        if (!eyes.TryGetFixationPoint(out Vector3 convergencePoint)) return Camera.main.transform.forward;
        if (convergencePoint == Vector3.zero) return Camera.main.transform.forward;
        
        eyes.TryGetLeftEyePosition(out Vector3 left);
        eyes.TryGetRightEyePosition(out Vector3 right);
        Vector3 center = (right + left) / 2f;

        Vector3 gazeDirection = (convergencePoint - center).normalized;
        
        if (Camera.main.transform.parent != null)
            gazeDirection = Camera.main.transform.parent.TransformDirection(gazeDirection);
        Debug.Log("iXRLib - " + gazeDirection);
        return gazeDirection;
    }
}
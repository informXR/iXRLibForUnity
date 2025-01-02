using System.Collections.Generic;
using System.Globalization;
using iXRLib;
using UnityEngine;
using UnityEngine.XR;

public interface ITrackInputDevices
{
    void Initialize(IIxrService ixrService);
}

[DefaultExecutionOrder(100)]
public class TrackInputDevices : MonoBehaviour, ITrackInputDevices
{
    private IIxrService _ixrService;
    private IConfigurationService _configService;
    private float _positionUpdateIntervalSeconds;
    
    private InputDevice _rightController;
    private InputDevice _leftController;
    private InputDevice _hmd;

    private const string HmdName = "Head";
    private const string RightControllerName = "Right Controller";
    private const string LeftControllerName = "Left Controller";

    private readonly Dictionary<InputFeatureUsage<bool>, bool> _rightTriggerValues = new();
    private readonly Dictionary<InputFeatureUsage<bool>, bool> _leftTriggerValues = new();

    public void Initialize(IIxrService ixrService)
    {
        _ixrService = ixrService;
        _configService = ServiceLocator.GetService<IConfigurationService>();
        _positionUpdateIntervalSeconds = (float)(60.0 / _configService.GetConfiguration().trackingUpdatesPerMinute);
        InvokeRepeating(nameof(InitializeInputDevices), 0, 1);
        InvokeRepeating(nameof(UpdateLocationData), 0, _positionUpdateIntervalSeconds);
    }

    private void Update()
    {
        iXRBase.CaptureTimeStamp();
        CheckTriggers();
        iXRBase.UnCaptureTimeStamp();
    }

    private void UpdateLocationData()
    {
        UpdateLocationData(_rightController);
        UpdateLocationData(_leftController);
        UpdateLocationData(_hmd);
    }

    private void UpdateLocationData(InputDevice device)
    {
        if (!device.isValid) return;
        
        device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        string deviceName = HmdName;
        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right)) deviceName = RightControllerName;
        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left)) deviceName = LeftControllerName;

        var positionDict = new Dictionary<string, string>
        {
            ["x"] = position.x.ToString(CultureInfo.InvariantCulture),
            ["y"] = position.y.ToString(CultureInfo.InvariantCulture),
            ["z"] = position.z.ToString(CultureInfo.InvariantCulture)
        };
        _ixrService.TelemetryEntry(deviceName + " Position", positionDict);
    }

    private void CheckTriggers()
    {
        CheckTriggers(CommonUsages.primaryButton);
        CheckTriggers(CommonUsages.secondaryButton);
        CheckTriggers(CommonUsages.triggerButton);
        CheckTriggers(CommonUsages.gripButton);
    }

    private void CheckTriggers(InputFeatureUsage<bool> trigger)
    {
        if (_rightController.isValid)
        {
            _rightController.TryGetFeatureValue(trigger, out bool pressed);
            _rightTriggerValues.TryGetValue(trigger, out bool current);
            if (pressed != current)
            {
                string action = "Pressed";
                if (!pressed) action = "Released";
                var telemetryData = new Dictionary<string, string>
                {
                    [trigger.name] = action
                };
                _ixrService.TelemetryEntry($"Right Controller {trigger.name}", telemetryData);
                _rightTriggerValues[trigger] = pressed;
            }
        }

        if (_leftController.isValid)
        {
            _leftController.TryGetFeatureValue(trigger, out bool pressed);
            _leftTriggerValues.TryGetValue(trigger, out bool current);
            if (pressed != current)
            {
                string action = "Pressed";
                if (!pressed) action = "Released";
                var telemetryData = new Dictionary<string, string>
                {
                    [trigger.name] = action
                };
                _ixrService.TelemetryEntry($"Left Controller {trigger.name}", telemetryData);
                _leftTriggerValues[trigger] = pressed;
            }
        }
    }
    
    private void InitializeInputDevices()
    {
        if (!_rightController.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right,
                ref _rightController);
        }

        if (!_leftController.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left,
                ref _leftController);
        }

        if (!_hmd.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref _hmd);
        }
    }

    private static void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);
        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
    }
}
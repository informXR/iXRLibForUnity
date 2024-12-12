using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[DefaultExecutionOrder(100)]
[AddComponentMenu("informXR/Track Object")]
public class TrackObject : MonoBehaviour
{
    private Vector3 _currentPosition;
    private Quaternion _currentRotation;
    private IIxrService _ixrService;

    private void Start()
    {
        _ixrService = ServiceLocator.GetService<IIxrService>();
        float positionUpdateIntervalSeconds = (float)(60.0 / ServiceLocator.GetService<IConfigurationService>().GetConfiguration().trackingUpdatesPerMinute);
        InvokeRepeating(nameof(UpdateLocation), 0, positionUpdateIntervalSeconds);
    }

    private void UpdateLocation()
    {
        if (transform.position.Equals(_currentPosition)) return;
        if (transform.rotation.Equals(_currentRotation)) return;

        _currentPosition = transform.position;
        _currentRotation = transform.rotation;
        var positionDict = new Dictionary<string, string>
        {
            ["x"] = transform.position.x.ToString(CultureInfo.InvariantCulture),
            ["y"] = transform.position.y.ToString(CultureInfo.InvariantCulture),
            ["z"] = transform.position.z.ToString(CultureInfo.InvariantCulture)
        };
        _ixrService.TelemetryEntry(name + " Position", positionDict);
    }
}

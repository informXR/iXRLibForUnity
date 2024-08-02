using iXRLib;
using UnityEngine;

[DefaultExecutionOrder(100)] // Doesn't matter when this one runs
[AddComponentMenu("InformXR/Track Object")]
public class TrackObject : MonoBehaviour
{
    private Vector3 _currentPosition;
    private Quaternion _currentRotation;

    private void Start()
    {
        float positionUpdateIntervalSeconds = (float)(60.0 / Configuration.instance.trackingUpdatesPerMinute);
        InvokeRepeating(nameof(UpdateLocation), 0, positionUpdateIntervalSeconds);
    }

    private void UpdateLocation()
    {
        if (transform.position.Equals(_currentPosition)) return;
        if (transform.rotation.Equals(_currentRotation)) return;

        _currentPosition = transform.position;
        _currentRotation = transform.rotation;
        iXRSend.AddTelemetryEntry(
            name + " Position", $"x,{transform.position.x},y,{transform.position.y},z,{transform.position.z}");
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCanvas : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private float _originalZ;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalZ = _rectTransform.position.z;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!eventData.pointerCurrentRaycast.isValid) return;
        
        _rectTransform.position = eventData.pointerCurrentRaycast.worldPosition;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        // Keep Z position constant
        Vector3 position = _rectTransform.position;
        position.z = _originalZ;
        _rectTransform.position = position;
        
        _rectTransform.LookAt(Camera.main.transform);
        _rectTransform.Rotate(0, 180, 0); // Flip it to face the right direction
    }
}
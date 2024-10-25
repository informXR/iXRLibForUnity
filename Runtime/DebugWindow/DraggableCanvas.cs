using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCanvas : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private float _originalZ;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalZ = _rectTransform.localPosition.z;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!eventData.pointerCurrentRaycast.isValid) return;
        
        _rectTransform.position = eventData.pointerCurrentRaycast.worldPosition;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        // Keep Z position constant
        Vector3 position = _rectTransform.localPosition;
        position.z = _originalZ;
        _rectTransform.localPosition = position;
        
        _rectTransform.LookAt(Camera.main.transform);
        _rectTransform.Rotate(0, 180, 0); // Flip it to face the right direction
    }
}
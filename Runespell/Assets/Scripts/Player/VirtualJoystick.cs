using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private RectTransform joystickBackground;
    [SerializeField] private RectTransform joystickHandle;

    private Vector2 joystickInput = Vector2.zero;
    private Vector2 startPos;

    public Vector2 JoystickInput => joystickInput.normalized;

    private void Start()
    {
        startPos = joystickBackground.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        joystickInput = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - (Vector2)joystickBackground.position;
        joystickInput = Vector2.ClampMagnitude(delta, joystickBackground.rect.width / 2);
        joystickHandle.localPosition = joystickInput;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        joystickHandle.localPosition = Vector3.zero;
        joystickInput = Vector2.zero;
    }
}

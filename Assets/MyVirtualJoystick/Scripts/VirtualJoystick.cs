using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    private Image joyBg;
    private Image stick;
    private Vector3 direction;

    void Start () {
        joyBg = GetComponent<Image>();
        stick = transform.GetChild(0).GetComponent<Image>();
	}

    public virtual void OnDrag(PointerEventData eventData) {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joyBg.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joyBg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joyBg.rectTransform.sizeDelta.y);
            direction = new Vector3(pos.x * 2 - 1, 0, pos.y * 2 - 1);
            direction = (direction.magnitude > 1 ? direction.normalized : direction);
            stick.rectTransform.anchoredPosition = new Vector3(direction.x * joyBg.rectTransform.sizeDelta.x / 3, direction.z * joyBg.rectTransform.sizeDelta.y / 3,0);
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData) {
        OnDrag(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData) {
        direction = Vector3.zero;
        stick.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float VJHorizontalAxis() {
        if (direction.x != 0)
            return direction.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float VJVerticalAxis() {
        if (direction.x != 0)
            return direction.z;
        else
            return Input.GetAxis("Vertical");
    }
}

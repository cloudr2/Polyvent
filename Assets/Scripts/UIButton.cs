using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private bool isPressed;
    public bool IsPressed { get { return isPressed; } }


    public void OnPointerDown(PointerEventData eventData) {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        isPressed = false;
    }
}
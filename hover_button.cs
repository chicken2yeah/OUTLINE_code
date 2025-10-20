using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class hover_button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Sprite mouseOn;
    public Sprite mouseOff;
    public Image Image;
    public bool isPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Image.sprite = mouseOn;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Image.sprite = mouseOff;
    }
}

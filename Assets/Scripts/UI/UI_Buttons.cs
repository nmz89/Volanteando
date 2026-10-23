using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Buttons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _image;
    [SerializeField] public Sprite _default, _pressed;

    public void SetSprites(Sprite defaultSprite, Sprite pressedSprite)
    {
        _default = defaultSprite;
        _pressed = pressedSprite;
        _image.sprite = _default;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _image.sprite = _pressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _image.sprite = _default;
    }
}
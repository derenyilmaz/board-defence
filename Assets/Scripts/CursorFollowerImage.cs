using System;
using UnityEngine;
using UnityEngine.UI;

public class CursorFollowerImage : MonoBehaviour
{ 
    private Image _image;

    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
    }

    public void SetSpriteAndShow(Sprite sprite)
    {
        _image.sprite = sprite;
        _image.enabled = true;
    }
    public void Hide()
    {
        _image.enabled = false;
    }
}

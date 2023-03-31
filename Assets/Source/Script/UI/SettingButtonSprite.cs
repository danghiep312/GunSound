using System;
using UnityEngine;
using UnityEngine.UI;


public class SettingButtonSprite : MonoBehaviour
{
    private Image _image;
    [Tooltip("0 is off, 1 is on")]
    public Sprite[] buttonSprite;

    private bool status;

    private void Awake()
    {
        _image = GetComponent<Image>();
        Update();
    }

    private void Update()
    {
        status = gameObject.name switch
        {
            "Vibration" => GameManager.Instance.vibrationOn,
            "Effect" => GameManager.Instance.effectOn,
            "Flash" => GameManager.Instance.flashOn,
            _ => true
        };

        _image.sprite = buttonSprite[status ? 1 : 0];
    }
}

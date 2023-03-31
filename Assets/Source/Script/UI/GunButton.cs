using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public enum ButtonType
{
    Normal,
    Gold,
    Lock,
}

public class GunButton : MonoBehaviour
{
    public int id;
    public Image bg;
    public Image gun;
    public TextMeshProUGUI bullet;
    public TextMeshProUGUI gunName;
    public ButtonType buttonType;
    
    public void InitButton(GunData data)
    {
        id = data.id;
        gun.sprite = data.gunSprite;
        gunName.text = data.gunName;
        bullet.text = data.bullet.ToString();
        buttonType = data.gunType;
        bg.sprite = buttonType == ButtonType.Gold ? SpriteMachine.Instance.goldPanel : SpriteMachine.Instance.normalPanel;
        if (gunName.text.Equals("Flash", StringComparison.OrdinalIgnoreCase))
        {
            gun.transform.localScale = Vector3.one * 0.6f;
        }
        if (gunName.text.Equals("Bomb", StringComparison.OrdinalIgnoreCase))
        {
            gun.transform.localScale = Vector3.one * 0.65f;
            gun.transform.rotation = Quaternion.Euler(Vector3.forward * 90f);
            gun.GetComponent<RectTransform>().anchoredPosition = Vector2.up * -25f;
        }

        if (gunName.text.Equals("Grenade", StringComparison.OrdinalIgnoreCase))
        {
            gun.transform.rotation = Quaternion.Euler(Vector3.forward * 90f);
            gun.GetComponent<RectTransform>().anchoredPosition = Vector2.up * -30f;
        }
        if (gunName.text.Equals("M32", StringComparison.OrdinalIgnoreCase))
        {
            gun.transform.localScale = Vector3.one * 0.7f;
        }
        
        if (gunName.text.Equals("ETERNAL CRUSADE", StringComparison.OrdinalIgnoreCase))
        {
            gun.transform.localScale = Vector3.one * 0.8f;
        }
            
    }
    
    public void ButtonPressed()
    {
        this.PostEvent(EventID.GunPlay, id);
    }
    
   
}

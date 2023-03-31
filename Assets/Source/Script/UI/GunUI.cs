using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    private int bulletCount;
    public GameObject gun;
    public TextMeshProUGUI ammoText;
    public Image[] ammo;
    public GameObject[] modeButtons;
    public RectTransform weatherZone;
    private bool isHidingUI;
    public Image hideUIButton;
    public Sprite[] hideSprites;
    public GameObject reloadPanel;
    public GameObject bulletZone;
    
    private void OnEnable()
    {
        isHidingUI = false;
        GetGun();
        weatherZone.anchoredPosition = new Vector2(110f, weatherZone.anchoredPosition.y);
        foreach (var mode in modeButtons)
        {
            mode.transform.GetChild(0).gameObject.SetActive(false);
        }
        modeButtons[0].transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Start()
    {
        this.RegisterListener(EventID.Fire, (param) => OnFire());
        this.RegisterListener(EventID.NoBullet, (param) => ShowReloadPanel());
    }

    private void OnFire()
    {
        bulletCount--;
        ammoText.text = "X" + bulletCount;
        RecheckAmmo();
    }

    private async void GetGun()
    {
        while (gun == null)
        {
            gun = GameObject.FindGameObjectWithTag("Player");
            await UniTask.Yield();
        }
        bulletCount = gun.GetComponent<Gun>().bulletNumber;
        ammoText.text = "X" + bulletCount;
        RecheckAmmo();
        bulletZone.SetActive(gun.GetComponent<Gun>().gunType != GunType.Laser);
    } 
    
    private void RecheckAmmo()
    {
        for (int i = 0; i < 10; i++)
        {
            ammo[i].gameObject.SetActive(i < bulletCount);
        }
        ammoText.text = "X" + bulletCount;
    }
    
    public void SetCurrentMode(GameObject button)
    {
        foreach (var mode in modeButtons)
        {
            mode.transform.GetChild(0).gameObject.SetActive(false);
        }
        
        gun.GetComponent<Gun>().currentMode = button.name switch
        {
            "Single" => FireMode.Single,
            "Burst" => FireMode.Burst,
            "Auto" => FireMode.Auto,
            _ => FireMode.Shake
        };
        
        button.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Reload()
    {
        var g = gun.GetComponent<Gun>();
        bulletCount = g.Reload();
        if (g.gunType == GunType.Shotgun)
        {
            AudioManager.Instance.Play("ShotgunReload");
        }
        else if (g.gunType != GunType.Bomb)
        {
            AudioManager.Instance.Play("HandgunReload");
        }
        RecheckAmmo();
    }
    
    public void OpenWeather(Button button)
    {
        button.interactable = false;
        if (weatherZone.anchoredPosition.x > 0)
        {
            weatherZone.DOAnchorPosX(-110f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                button.interactable = true;
            });
        }
        else
        {
            weatherZone.DOAnchorPosX(110f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                button.interactable = true;
            });
        }
    }

    public void ToggleUI()
    {
        if (isHidingUI)
        {
            isHidingUI = false;
            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

            hideUIButton.sprite = hideSprites[1];
        }
        else
        {
            isHidingUI = true;
            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            hideUIButton.sprite = hideSprites[0];
        }
    }
    
    public void ShowReloadPanel()
    {
        reloadPanel.SetActive(true);
    }
}

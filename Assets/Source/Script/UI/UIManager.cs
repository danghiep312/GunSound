using System;
using UnityEngine;
using UnityEngine.Serialization;


public class UIManager : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject inventoryPanel;
    public GameObject gunUi;
    public GameObject bombUi;
    public GameObject currentPanel;
    public GameObject[] holders;

    private void Awake()
    {
        
    }

    private void Start()
    {
        this.RegisterListener(EventID.GunPlay, (param) => PlayGunGame());
        this.RegisterListener(EventID.BombPlay, (param) => PlayBombGame());
        bombUi.SetActive(false);
        currentPanel = homePanel;
    }

    private void PlayBombGame()
    {
        SetCurrentPanel(bombUi);
    }

    private void PlayGunGame()
    {
        SetCurrentPanel(gunUi);
    }
    
    private void SetCurrentPanel(GameObject panel)
    {
        currentPanel.SetActive(false);
        currentPanel = panel;
        currentPanel.SetActive(true);
    }
    
    public void OpenInventory(int id) // 0 is gun, 1 is saber, 2 is bomb
    {
        SetCurrentPanel(inventoryPanel);
        Array.ForEach(holders, g => g.SetActive(false));
        holders[id].SetActive(true);
    }
    
    public void BackToInventory()
    {
        SetCurrentPanel(inventoryPanel);
        this.PostEvent(EventID.BackGamePlay);
    }

    public void BackToHome()
    {
        SetCurrentPanel(homePanel);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool vibrationOn;
    public bool effectOn;
    public bool flashOn;

    private void Start()
    {
        vibrationOn = PlayerPrefs.GetInt("Vibration", 1) == 1;
        effectOn = PlayerPrefs.GetInt("Effect", 1) == 1;
        flashOn = PlayerPrefs.GetInt("Flash", 1) == 1;
    }

    #region Toggle Function

    public void ToggleVibration()
    {
        vibrationOn = !vibrationOn;
        PlayerPrefs.SetInt("Vibration", vibrationOn ? 1 : 0);
    }

    public void ToggleEffect()
    {
        effectOn = !effectOn;
        PlayerPrefs.SetInt("Effect", effectOn ? 1 : 0);
    }
    
    public void ToggleFlash()
    {
        flashOn = !flashOn;
        PlayerPrefs.SetInt("Flash", flashOn ? 1 : 0);
    }
    
    #endregion
}

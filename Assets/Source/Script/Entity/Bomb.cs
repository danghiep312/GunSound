using System;
using TMPro;
using UnityEngine;


public class Bomb : MonoBehaviour
{
    public float countdown = 10f;
    public TextMeshProUGUI time;
    
    public bool isPlanted = false;

    private void Awake()
    {
        if (isPlanted) return;
        this.RegisterListener(EventID.Plant, (param) => OffSetPlant((float) param));
    }

    private void OffSetPlant(float countTime)
    {
        countdown = countTime;
        isPlanted = true;
    }

    public void AddTime()
    {
        if (isPlanted) return;
        countdown++;
    }

    public void SubTime()
    {
        if (countdown >= 2f)
        {
            countdown--;
        }
    }

    private void Update()
    {
        int minutes = (int)countdown / 60;
        int seconds = (int)countdown % 60;
        time.text = $"{minutes:00}:{seconds:00}";
        if (isPlanted)
        {
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
            }
            if (countdown < 0)
            {
                countdown = 0;
                this.PostEvent(EventID.Explosion);
            }
        }
    }

    public void Plant()
    {
        if (isPlanted) return;
        isPlanted = true;
        this.PostEvent(EventID.Plant, countdown);
    }
    
    public void Reset()
    {
        countdown = 10f;
        isPlanted = false;
    }
}

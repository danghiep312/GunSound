using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombUI : MonoBehaviour
{
    public BombButtonHolder bombButtonHolder;
    public Image bombIcon;
    public GameObject bomb;
    public TextMeshProUGUI description;
    public ParticleSystem explosion;
    public GameObject brokenScreen;
    public GameObject[] buttonHolder;
    public GameObject backButton;
    private string explosionSound;

    private bool postedEvent;
    private bool isPlanted;

    private void Awake()
    {
        this.RegisterListener(EventID.BombPlay, (param) => InitBomb((int) param));
        this.RegisterListener(EventID.Plant, (param) => PlantBomb((float)param));
    }
    

    public void OnEnable()
    {
        isPlanted = false;
        if (!description) description = GetComponentInChildren<TextMeshProUGUI>();
        description.transform.DOKill();
        description.transform.localScale = Vector3.one * 1.1f;
        description.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            description.transform.DOScale(Vector3.one * 1.1f, 0.5f).SetEase(Ease.InQuad).SetLoops(-1, LoopType.Yoyo);
        });
        
    }

    private void OnDisable()
    {
        bombIcon.color = Color.white;
        description.transform.localScale = Vector3.one * 1.1f;
        ResetStatus();
        DestroyImmediate(bomb);
        
    }

    private void InitBomb(int id)
    {
        var bombPrefab = Resources.Load<GameObject>("Prefabs/Bombs/" + id);
        if (bombPrefab)
        {
            bomb = Instantiate(bombPrefab, transform.transform);
        }

        if (bomb)
        {
            bomb.transform.SetSiblingIndex(3);
            bombIcon.sprite = null;
            bombIcon.color = new Color(1, 1, 1, 0);
        }
        else
        {
            (bombIcon.sprite, explosionSound) = bombButtonHolder.GetBombAndSound(id);
        }
    }
    
    public void PlantBomb(float countdown)
    {
        if (isPlanted) return;
        if (!postedEvent)
        {
            postedEvent = true;
            this.PostEvent(EventID.Plant, countdown);
        }

        isPlanted = true;
        AudioManager.Instance.Play("Plant");
        Util.Delay(0.5f, () => AudioManager.Instance.Play("Countdown"));
        bombIcon.transform.DOScale(Vector3.one * .9f, 1f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        bomb.transform.DOScale(Vector3.one * .9f, 1f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        description.gameObject.SetActive(false);
        Array.ForEach(buttonHolder, b =>
        {
            if (!b.name.Equals("" + (int)countdown)) b.SetActive(false);
        });
        Util.Delay(countdown, () =>
        {
            if (!isPlanted) return;
            Explode();
            Util.Delay(.25f, () =>
            {
                brokenScreen.SetActive(true);
                ResetStatus();
            });
        });
        
    }

    private void ResetStatus()
    {
        if (bomb)
        {
            bomb.GetComponent<Bomb>().Reset();
            bomb.transform.DOKill();
            bomb.transform.localScale = Vector3.one;
        }
        postedEvent = false;
        isPlanted = false;
        
        bombIcon.transform.DOKill();
        bombIcon.transform.localScale = Vector3.one;
        description.gameObject.SetActive(true);
        Array.ForEach(buttonHolder, button => button.SetActive(true));
    }

    private void Explode()
    {
        AudioManager.Instance.Play(explosionSound);
        AudioManager.Instance.Play("GlassBreak");
        AudioManager.Instance.Stop("Countdown");
        Debug.Log("stop");
        explosion.Play();
    }

    public void ClickBack()
    {
        ResetStatus();
        AudioManager.Instance.StopAll();
    }
}

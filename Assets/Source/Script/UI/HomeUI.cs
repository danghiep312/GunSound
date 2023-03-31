using System;
using DG.Tweening;
using UnityEngine;


public class HomeUI : MonoBehaviour
{
    [SerializeField] private ParticleSystem highlight;
    private float delayTime;
    public RectTransform[] buttons;
    public RectTransform title;

    private void Start()
    {
        title.DOLocalMoveX(65f, 0.5f).SetEase(Ease.OutQuad);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].DOLocalMoveX(0f, 1f).SetDelay(i * 0.2f).SetEase(Ease.OutBack);
        }
    }

    private void Update()
    {
        if (delayTime > 0)
        {
            delayTime -= Time.deltaTime;
        }
        else
        {
            highlight.Play();
            delayTime = 2f;
        }
    }
}

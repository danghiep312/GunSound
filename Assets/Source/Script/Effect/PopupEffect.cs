using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;


public class PopupEffect : MonoBehaviour
{
    [Title("Appear Effect")]
    public float duration;
    public float delay;
    public Ease ease;
    
    [Title("Disappear Effect")]
    public float disDuration;
    public float disDelay;
    public Ease disEase;
    public GameObject targetDisable;
    
    [Title("Appear Callback")]
    [ShowInInspector]
    public List<Action> appearCallback;
    
    [ShowInInspector]
    [Title("Disappear Callback")]
    public List<Action> disappearCallback;
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, duration).SetDelay(delay).SetEase(ease).OnComplete(() =>
        {
            if (appearCallback != null)
            {
                foreach (var callback in appearCallback)
                {
                    callback?.Invoke();
                }
            } 
        });
    }

    public void Disappearance()
    {
        transform.DOScale(Vector3.zero, disDuration).SetDelay(disDelay).SetEase(disEase).OnComplete(() =>
        {
            if (disappearCallback != null)
            {
                foreach (var callback in disappearCallback)
                {
                    callback?.Invoke();
                }
            }
            targetDisable.SetActive(false);
        });
    }

}

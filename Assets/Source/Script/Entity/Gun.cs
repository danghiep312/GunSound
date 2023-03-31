using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public enum FireMode {
    Single,
    Burst,
    Auto,
    Shake
}

public enum GunType
{
    Rifle,
    Snipe,
    Shotgun,
    Pistol,
    Bomb,
    Laser,
}
public class Gun : MonoBehaviour
{
    public int ID;
    private GameObject spark;
    private SpriteRenderer flash;
    public FlashlightPlugin flashlight;
    public ParticleSystem smoke;
    public ParticleSystem explosion;
    public ParticleSystem shotgunBullet;

    [Title("Fire Rate Each Mode")] [Tooltip("0 is Single, 1 is Burst, 2 is Auto, 3 is Shake")] [SerializeField]
    private float[] fireRates = new float[4];

    public FireMode currentMode;
    public GunType gunType;
    
    [Title("Fire Config")] 
    // For config
    public float fireRate;
    public float recoilTime;
    public float flashTime;
    public float sparkTime;
    public int burstNumber;
    public int bulletNumber;

    // For code
    private float fireTime;
    private int burstCount;
    private int bulletCount;
    

    private void OnEnable()
    {
        bulletCount = bulletNumber;
        currentMode = FireMode.Single;
    }

    private void Start()
    {
        var ids = gameObject.name.Substring(0, 2);
        ID = int.Parse(ids);
        spark = transform.GetChild(0).gameObject;
        smoke = GameObject.FindGameObjectWithTag("GunSmoke").GetComponent<ParticleSystem>();
        flash = GameObject.FindGameObjectWithTag("Flash").GetComponent<SpriteRenderer>();
        flashlight = GameObject.FindGameObjectWithTag("FlashLight").GetComponent<FlashlightPlugin>();
        explosion = GameObject.FindGameObjectWithTag("Explosion").GetComponent<ParticleSystem>();

        if (gunType == GunType.Laser) spark.transform.parent = null;
        if (gunType == GunType.Shotgun) shotgunBullet = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        fireRate = GetFireRateByMode(currentMode);
        if (fireTime > 0) fireTime -= Time.deltaTime;
        if (currentMode == FireMode.Single)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CheckCanFire())
                {
                    Fire();
                    ReleaseSmoke(1f);
                }
            } 
            return;
        }
        
        if (currentMode == FireMode.Auto)
        {
            if (Input.touchCount > 0)
            {
                if (CheckCanFire())
                {
                    Fire();
                }
            }
        }
        else if (currentMode == FireMode.Burst)
        {
            if (Input.touchCount > 0 && burstCount > 0)
            {
                if (CheckCanFire())
                {
                    Fire();
                    burstCount--;
                }
            }
           

            if (Input.GetMouseButtonUp(0)) burstCount = burstNumber;
        }
        else
        {
            if (ShakeDetector.Instance.isShaking)
            {
                if (CheckCanFire())
                {
                    Fire();
                }
            }
        }
        

        if (Input.GetMouseButtonUp(0) || (!ShakeDetector.Instance.isShaking && currentMode == FireMode.Shake) || bulletCount <= 0 ||
            (currentMode == FireMode.Burst && burstCount <= 0))
        {
            ReleaseSmoke(0.2f);
        }
        
        
    }

    private async void ReleaseSmoke(float seconds)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(seconds));
        smoke.transform.position = Vector3.one * 100f;
    }

    private void Fire()
    {
        if (bulletCount < 0) return;
        if (bulletCount == 0)
        {
            AudioManager.Instance.Play("FireEmpty");
        }
        
        if (GameManager.Instance.vibrationOn)
        {
            MMVibrationManager.TransientHaptic(.5f, .65f);
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        if (GameManager.Instance.flashOn)
        {
            Flash();
        }
#endif
        if (gunType != GunType.Laser)
        {
            flash.DOFade(1f, flashTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (GameManager.Instance.effectOn)
                {
                    spark.transform.localScale = Vector3.right * 3f + Vector3.up * 2f;
                    spark.transform.DOScale(Vector3.zero, sparkTime).SetEase(Ease.OutSine).SetUpdate(true);
                }
                flash.DOFade(0, flashTime).SetEase(Ease.Linear).SetUpdate(true);
            });
            bulletCount--;
        }
        else
        {
            LaserFire();
        }
        fireTime = fireRate;
        this.PostEvent(EventID.Fire);
        if (bulletCount <= 0)
        {
            this.PostEvent(EventID.NoBullet);
        }
       
        FireSound(gunType);
        
        
        if (ID == 22)
        {
            flash.sortingOrder = 100;
            Explosion();
        }
        else
        {
            flash.sortingOrder = 0;
        }
        
        if (gunType == GunType.Bomb) return;



        if (gunType != GunType.Laser)
        {
            if (smoke.transform.position != spark.transform.position)
            {
                smoke.Play();
                smoke.transform.position = spark.transform.position;
            }
        }
        
        if (gunType == GunType.Shotgun) shotgunBullet.Play();
        
        spark.transform.DOKill();
        transform.DORotate(Vector3.forward * -92f, recoilTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DORotate(Vector3.forward * -90f, recoilTime).SetEase(Ease.Linear).SetUpdate(true);
        }).SetUpdate(true);
        
    }

    private async void LaserFire()
    {
        spark.SetActive(true);
        spark.transform.rotation = Quaternion.Euler(Vector3.forward * -90f);
        await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
        spark.SetActive(false);
    }
    
    private void FireSound(GunType type)
    {
        switch (gunType)
        {
            case GunType.Shotgun:
                AudioManager.Instance.Play("Shotgun");
                break;
            case GunType.Pistol:
                AudioManager.Instance.Play("Pistol");
                break;
            case GunType.Snipe:
                AudioManager.Instance.Play("Snipe");
                break;
            case GunType.Bomb:
                if (ID == 19) AudioManager.Instance.Play("Flashbang");
                if (ID == 22) AudioManager.Instance.Play("BigBomb");
                if (ID == 24) AudioManager.Instance.Play("Grenade");
                break;
            case GunType.Laser:
                AudioManager.Instance.Play("Laser");
                break;
            default:
                if (ID == 23) AudioManager.Instance.Play("MiniFire");
                else if (ID == 25) AudioManager.Instance.Play("M32");
                else
                    AudioManager.Instance.Play("Rifle");
                break;
        }
    }

    private bool CheckCanFire()
    {
        if (fireTime <= 0)
        {
            if (currentMode == FireMode.Shake) return true;
            if (!Util.ClickOverUI()) return true;
        }
        
        return false;
    }

    private float GetFireRateByMode(FireMode mode)
    {
        return mode switch
        {
            FireMode.Single => fireRates[0],
            FireMode.Burst => fireRates[1],
            FireMode.Auto => fireRates[2],
            _ => fireRates[3]
        };
    }

    private async void Flash()
    {
        flashlight.TurnOn();
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        flashlight.TurnOff();
    }

    private async void Explosion()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1.2f));
        explosion.Play();
    }
    public int Reload()
    {
        bulletCount = bulletNumber;
        return bulletCount;
    }

    private void OnDestroy()
    {
        DestroyImmediate(spark);
    }
}

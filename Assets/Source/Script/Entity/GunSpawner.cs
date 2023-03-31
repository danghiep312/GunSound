using System;
using UnityEngine;


    public class GunSpawner : MonoBehaviour
    {
        public GameObject currentGun;
        
        private void Start()
        {
            this.RegisterListener(EventID.GunPlay, (param) => SpawnGun((int)param));
            this.RegisterListener(EventID.BackGamePlay, (param) => BackGamePlay());
        }

        private void BackGamePlay()
        {
            if (currentGun)
            {
                currentGun.gameObject.SetActive(false);
                DestroyImmediate(currentGun);
            }
        }

        private void SpawnGun(int id)
        {
            var gunPrefab = Resources.Load<GameObject>("Prefabs/Guns/" + id);
            var gun = Instantiate(gunPrefab, Vector3.zero, Quaternion.identity);
            gun.transform.rotation = Quaternion.Euler(Vector3.forward * -90f);
            currentGun = gun;
        }
        
        
    }

using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class ButtonClick : MonoBehaviour
    {
        public string clickSound;
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(Play);
        }

        private void Play()
        {
            AudioManager.Instance.Play(clickSound);
        }
    }

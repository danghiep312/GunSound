using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class GunButtonHolder : MonoBehaviour
{
    public GunData[] gunsData;
    public GameObject buttonPrefab;
    
    [SerializeField] private GridLayoutGroup grid;

    private List<GunButton> gunButtons;
    
    private void Start()
    {
        gunsData = Resources.LoadAll<GunData>("Guns Data");
        gunsData = (from data in gunsData orderby data.id select data).ToArray();
        gunButtons = new List<GunButton>();
        Util.ActivateLastFrame(Init);
    }

    private void Init()
    {
        foreach (var data in gunsData)
        {
            var button = Instantiate(buttonPrefab, grid.transform).GetComponent<GunButton>();
            button.InitButton(data);
            gunButtons.Add(button);
        }

        var width = GetComponent<RectTransform>().rect.width;
        Debug.Log(width);

        grid.cellSize = Vector2.one * (width - grid.spacing.x) / 2f;
    }
}

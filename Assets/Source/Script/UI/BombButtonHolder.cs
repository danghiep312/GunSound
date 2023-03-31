using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class BombButtonHolder : MonoBehaviour
{
    public BombData[] bombsData;
    public GameObject buttonPrefab;
    
    [SerializeField] private GridLayoutGroup grid;

    private List<BombButton> bombButtons;
    
    private void Start()
    {
        bombsData = Resources.LoadAll<BombData>("Bombs Data");
        bombsData = (from data in bombsData orderby data.id select data).ToArray();
        bombButtons = new List<BombButton>();
        Util.ActivateLastFrame(Init);
    }
    
    private void Init()
    {
        foreach (var data in bombsData)
        {
            var button = Instantiate(buttonPrefab, grid.transform).GetComponent<BombButton>();
            button.InitButton(data);
            bombButtons.Add(button);
        }

        var width = GetComponent<RectTransform>().rect.width;
        Debug.Log(width);

        grid.cellSize = Vector2.one * (width - grid.spacing.x) / 2f;
    }

    public (Sprite bombIcon, string explosionSoundName) GetBombAndSound(int id)
    { 
        foreach (var data in bombsData)
        {
            if (data.id == id)
            {
                Debug.Log("Chay vao day");
                return (data.bombSprite, data.explosionSoundName);
            }
        }

        return (null, "");

    }
}

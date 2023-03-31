using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BombButton : MonoBehaviour
{
    public int id;
    public Image bg;
    public Image bomb;
    public ButtonType buttonType;
    public TextMeshProUGUI bombName;
    
    public void InitButton(BombData data)
    {
        id = data.id;
        buttonType = data.bombType;
        bomb.sprite = data.bombSprite;
        bg.sprite = buttonType == ButtonType.Gold ? SpriteMachine.Instance.goldPanel : SpriteMachine.Instance.normalPanel;
        //bombName.text = data.bombName;
    }
    
    public void ButtonPressed()
    {
        this.PostEvent(EventID.BombPlay, id);
    }
}

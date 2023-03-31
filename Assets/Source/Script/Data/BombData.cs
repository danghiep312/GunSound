using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "BombData", menuName = "Assets/BombData", order = 0)]
public class BombData : ScriptableObject
{
    public int id;
    public string bombName;
    public string explosionSoundName;
    public ButtonType bombType;
    [PreviewField]
    public Sprite bombSprite;
}

using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Assets/GunData")]
public class GunData : ScriptableObject
{
    public int id;
    public ButtonType gunType;
    public string gunName;
    public int bullet;
    public Sprite gunSprite;
}

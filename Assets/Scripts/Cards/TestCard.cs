using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card", order = 1)]
public class TestCard : ScriptableObject
{
    public enum cardColors {white, green, red, black, blue};
    public cardColors color;
    public int mainValue;
    public int bonusValue;
}

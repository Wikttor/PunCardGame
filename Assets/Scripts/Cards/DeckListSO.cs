using UnityEngine;

[CreateAssetMenu(fileName = "DeckList", menuName ="Cards/DeckList", order = 0)]
public class DeckListSO : ScriptableObject
{
    public TestCard[] list;
}

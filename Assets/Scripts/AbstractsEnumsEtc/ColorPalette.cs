using UnityEngine;

[CreateAssetMenu(fileName = "Palette", menuName = "Cards/Palette", order = 1)]
public class ColorPalette : ScriptableObject
{
    public Color error = Color.cyan;
    public Color[] colors;

    public enum colorsEnum { white, green, red, black, blue, notOwned, owned, selected, bidSubmitted };

    public Color GetColor(colorsEnum name)
    {
        if (colors.Length >= (int)name && colors[(int)name] != null)
        {
            return colors[(int)name];
        }

        return error;
    }
}

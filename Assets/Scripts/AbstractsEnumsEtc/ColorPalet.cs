using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Palet", menuName = "Cards/Palet", order = 1)]
public class ColorPalet : ScriptableObject
{
    public Color error = Color.cyan;
    public Color[] colours;

    public enum coloursEnum { white, green, red, black, blue, notOwned, owned, selected, bidSubmitted };

    public Color GetColor (coloursEnum name)
    {
        if(colours.Length >= (int)name && colours[(int)name] != null)
        {
            return colours[(int)name];
        }
        else
        {
            return error;
        }
    }
}

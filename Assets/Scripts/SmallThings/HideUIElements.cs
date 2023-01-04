using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUIElements : MonoBehaviour
{
    public GameObject[] objectsToDisable;
    private bool areEnabled = true;

    public void DoDisablingOrEnabling()
    {
        areEnabled = !areEnabled;
        foreach (GameObject UIelement in objectsToDisable)           
            UIelement.SetActive(areEnabled);
    }
}

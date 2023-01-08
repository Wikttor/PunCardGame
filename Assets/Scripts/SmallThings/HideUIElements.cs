using UnityEngine;

public class HideUIElements : MonoBehaviour
{
    public GameObject[] objectsToDisable;
    private bool areEnabled = true;

    public void ToggleUIElements()
    {
        areEnabled = !areEnabled;
        foreach (GameObject UIelement in objectsToDisable)           
            UIelement.SetActive(areEnabled);
    }
}

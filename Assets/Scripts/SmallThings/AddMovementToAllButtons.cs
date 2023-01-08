using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddMovementToAllButtons : MonoBehaviour
{
    EventTriggerInterface eventTrigger;
    void Start()
    {
        eventTrigger = this.gameObject.AddComponent<EventTriggerInterface>();
        eventTrigger.AddEventTrigger(EventTriggerInterface.supportedEvents.OnMouseZeroDown, AddMovement);
    }

    public void AddMovement()
    {
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        List<GameObject> listOfObjects = new List<GameObject>();
        List<GameObject> objectsWithButtons = new List<GameObject>();

        foreach (GameObject singleObject in allObjects)
        {
            listOfObjects.Add(singleObject);
        }
        for (int i = 0; i < listOfObjects.Count; i++)
        {
            GameObject singleObject = listOfObjects[i];
            int childCount = singleObject.transform.childCount;

            for (int ii = 0; ii < singleObject.transform.childCount; ii++)
            {
                listOfObjects.Add(singleObject.transform.GetChild(ii).gameObject);
            }
            if (singleObject.GetComponent<Button>() != null &&
                singleObject.GetComponent<DragableButton>() == null)
            {
                objectsWithButtons.Add(singleObject);
            }
        }
        foreach (GameObject objectWithButton in objectsWithButtons)
        {
            objectWithButton.AddComponent<DragableButton>();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableButton : MonoBehaviour
{
    public List<GameObject> targets;
    bool isInitialised = false;
    public void OnEnable()
    {
        if (isInitialised)
        {
            return;
        }
        if (targets == null)
        {
            targets = new List<GameObject>();
        }
        if (this.transform.parent == null ||
            !targets.Contains(this.transform.parent.gameObject))
        {
            targets.Add(this.gameObject);
        }
        EventTriggerInterface ETReference = this.gameObject.AddComponent<EventTriggerInterface>();
        ETReference.AddEventTrigger(EventTriggerInterface.supportedEvents.OnMouseOneDown, StartMoving);
        isInitialised = true;
    }

    public void StartMoving()
    {
        StartCoroutine(KeepMoving());
    }

    IEnumerator KeepMoving()
    {
        Vector3 lastMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 mouseDelta;
        Vector3 newMousePositon;

        while (Input.GetMouseButton(1))
        {
            newMousePositon = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            mouseDelta = newMousePositon - lastMousePosition;
            lastMousePosition = newMousePositon;
            foreach (GameObject target in targets)
            {
                target.transform.Translate(mouseDelta);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}


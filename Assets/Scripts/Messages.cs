using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Messages : MonoBehaviour
{
    public static Messages instance;
    public TextMeshProUGUI myText;

    void Start()
    {
        instance = this;
        myText = this.GetComponent<TextMeshProUGUI>();
        if (myText == null)
        {
            Debug.Log("Couldn't get TMProUI component");
        }
    }

    public static void ClearMessages()
    {
        if (instance != null)
        {
            instance.myText.text = "";
        }
        else
        {
            Debug.Log("There is no Messages class instance");
        }
    }

    public static void DisplayMessage(string message)
    {
        if (instance != null)
        {
            instance.myText.text += "\n" + message;         
            if (message.Length > 0 &&               // this should be a bit more sophisticated
                message[message.Length - 1] != '.' ||
                message[message.Length - 1] != ':'
                )
            {
                instance.myText.text += ".";
            }
        }
        else
        {
            Debug.Log("There is no Messages class instance");
        }
    }

    public static void TimeStampedDisplayMessage(string message)
    {
        DisplayMessage(message + " at " + Time.time.ToString() );
    }

    public static void PUNConnectedSuccessfully()
    {
        DisplayMessage("Connected to server and joined lobby successfully");
    }
    public static void PUNCreatedRoom()
    {
        DisplayMessage("Successfully created a room");
    }
    public static void PUNJoinedRoom()
    {
        DisplayMessage("Succesfully joined a room");
    }
    public static void NoRPCCarrier()
    {
        DisplayMessage("No RPC Carrier on the scene");
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CardSpawner : NetworkedItem
{    
    public GameObject cardUIParent;
    public GameObject spawnParent, controlledCardsParent;
    public GameObject triggeringButton;
    public GameLogicNetworked logic;
    public Vector3 spawnOffset;
    public Vector3[] spawnPositions;
    public int spawnIterator = 0;
    public static CardSpawner instance;


    void Start()
    {
        instance = this;
        view = GetComponent<PhotonView>();
        punID = (int)view.ViewID;
        EventTriggerInterface.AddEventTriggerStatic(EventTriggerInterface.supportedEvents.OnMouseZeroDown,
            SpawnCard,
            triggeringButton);
    }
    public void SpawnCard()
    {
        if (!logic.isInitialised)
        {
            logic.InitDeckNPassViewID();
            PlayerManager.instance.NextPlayer();
        }
        GameLogicNetworked.instance.DrawNewCards();
    }
    public void SetCardsPosition(GameObject cardToReset)
    {
        cardToReset.transform.parent = cardUIParent.transform;
        cardToReset.transform.position = cardUIParent.transform.position + spawnPositions[spawnIterator];
        cardToReset.transform.localScale = new Vector3(1, 1, 1);//I dont know what is affecting the scale
        spawnIterator++;
        spawnIterator %= 3;
    }
}

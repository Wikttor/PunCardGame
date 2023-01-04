using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CardIdentifier : NetworkedItem
{
    Dictionary<TestCard, int> cardDict;
    TestCard[] cardTypesArray;
    int cardsRegisteredCount = 0;

    public void Start()
    {
        cardDict = new Dictionary<TestCard, int>();
        cardTypesArray = new TestCard[1000];
    }

    public void Register(TestCard card)
    {
        if (!cardDict.ContainsKey(card))
        {
            cardDict.Add(card, cardsRegisteredCount);
            cardTypesArray[cardsRegisteredCount] = card;
            cardsRegisteredCount++;
        }
    }

    public TestCard FindWithID(int cardID)
    {
        if (cardID <= cardsRegisteredCount)
        {
            return cardTypesArray[cardID];
        }
        else
        {
            Debug.Log("Card with ID passed was never registered");
            return null;
        }
    }
    public int GetID(TestCard card)
    {
        int cardID = -1;
        cardDict.TryGetValue(card,out cardID);
        return cardID;
    }
}

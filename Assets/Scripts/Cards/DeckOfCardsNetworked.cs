using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeckOfCardsNetworked : NetworkedItem
{
    PhotonView logicHolder;
    public DeckListSO deckList;
    public List<TestCard> deck;
    public CardSpawner cardSpawner;
    public int cardsDrawn = 0;
    
    [PunRPC]
    public void InitDeckRPC(int mainLoginObjectID)
    {
        logicHolder = PhotonView.Find(mainLoginObjectID);
        logicHolder.GetComponent<GameLogicNetworked>().SetDeckReference(this);
    }
    public void LoadDeck(GameObject deckInfoHandler)
    {
        this.view.RPC("LoadDeckRPC", RpcTarget.All, deckInfoHandler.GetComponent<PhotonView>().ViewID);
    }
    [PunRPC]
    public void LoadDeckRPC(int deckReferenceHolderID)
    {
        PhotonView ReferenceHolderView = PhotonView.Find((int)deckReferenceHolderID);
        deckList = ReferenceHolderView.GetComponent<GameLogicNetworked>().PassDeckList();
        deck = new List<TestCard>();
        foreach(TestCard card in deckList.list)
        {
            deck.Add(card);
            ReferenceHolderView.GetComponent<CardIdentifier>().Register(card);
        }
    }
    public void Shuffle()
    {
        int deckCount = deck.Count;
        int[] cardOrder = new int[deckCount];
        for(int i = 0; i < deckCount; i++)
        {
            cardOrder[i] = Random.Range(0, deck.Count - 1 - i);
        }
        view.RPC("ShuffleRPC", RpcTarget.All, cardOrder);
    }
    [PunRPC]
    public void ShuffleRPC(int[] cardOrder)
    {
        List<TestCard> tempList = new List<TestCard>();
        foreach (TestCard card in deck)
        {
            tempList.Add(card);
        }
        deck = new List<TestCard>();
        int deckCount = cardOrder.Length;
        for(int i = 0; i < deckCount; i++)
        {
            deck.Add(tempList[cardOrder[i]]);
            tempList.Remove(tempList[cardOrder[i]]);
        }
    }
    public void DrawAndInstantiateCard(CardSpawner spawner)
    {
        if(deck.Count > 0)
        {
            TestCard cardTypeSO = deck[0];
            this.view.RPC("CardDrawnRPC", RpcTarget.All);
            int cardTypeID = logicHolder.GetComponent<CardIdentifier>().GetID(cardTypeSO);
            GameObject newCardObject = PhotonNetwork.Instantiate("OneCard", Vector3.zero, Quaternion.identity);
            newCardObject.GetComponent<PhotonView>().RPC("InitCardRPC",
                RpcTarget.All, 
                spawner.punID,
                cardTypeID);
        }
    }
    [PunRPC]
    public void CardDrawnRPC()
    {
        cardsDrawn++;
        deck.RemoveAt(0);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameLogicNetworked : PunEventHandler
{
    public bool isInitialised = false;
    public static GameLogicNetworked instance;

    public DeckListSO deckList;
    public DeckOfCardsNetworked deck;
    public static int highestBid = -1;

    public int localPlayerPotentialBidValue = -1;

    public CardSpawner UIHandler;

    public Dictionary<int, TestCard> cardIdentifier;
    private List<CardNetworked> bidCardSet;
    //public List<CardNetworked> cardsSpawned;


    public DeckListSO PassDeckList()
    {
        return deckList;
    }

    private void Start()
    {
        bidCardSet = new List<CardNetworked>();
        instance = this;
        InitPhotonViewReferences();
        //cardsSpawned = new List<CardNetworked>();
    }

    public void InitDeckNPassViewID()
    {
        cardIdentifier = new Dictionary<int, TestCard>();
        if (deckList != null && deck == null)
        {

            GameObject deckInitialized = PhotonNetwork.Instantiate("DeckNetworked", new Vector3(0, 0, 0), Quaternion.identity);
            deckInitialized.GetComponent<PhotonView>().RPC("InitDeckRPC", RpcTarget.All, view.ViewID);
            deck.LoadDeck(this.gameObject);
            deck.Shuffle();
        }
        view.RPC("SetIsInitialisedRPC", RpcTarget.All);
    }

    public void SetDeckReference(DeckOfCardsNetworked deckReference)
    {
        deck = deckReference;
    }

    [PunRPC]
    public void SetIsInitialisedRPC()
    {
        isInitialised = true;
    }

    public void AddOrRemoveCardFromSet(CardNetworked cardSelected, bool add)
    {
        if (add)
        {
            bidCardSet.Add(cardSelected);
        }
        else
        {
            bidCardSet.Remove(cardSelected);
        }
        localPlayerPotentialBidValue = BidCalculator.CalculateMaxBid(bidCardSet);
        BidTool.SetValue = localPlayerPotentialBidValue;
    }

    public void PlayerFolded()
    {
        view.RPC("PlayerFoldedRPC", RpcTarget.All);
    }
    [PunRPC]
    public void PlayerFoldedRPC()
    {
        PlayerManager.instance.activePlayerInfo.folded = true;
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerInfo winner = CheckForWinner();
            if (winner != null)
            {
                PlayerManager.instance.SetActivePlayer(winner.view.ViewID);
                winner.SetNSyncCoins(winner.Coins - winner.coinsUsed);
                MyPunEvents.RaiseEndOfTurn(winner);
            }
            else
            {
                PlayerManager.instance.NextPlayer();
            }
        }
    }

    public PlayerInfo CheckForWinner()
    {
        PlayerInfo winner = null;
        int playersNotFolded = 0;
        foreach(PlayerInfo player in PlayerManager.players)
        {
            if (!player.folded)
            {
                playersNotFolded++;
                winner = player;
            }
        }
        if(playersNotFolded == 1)
        {
            return winner;
        }
        else
        {
            return null;
        }
    }
    public static void TryBid(int setValue, int coinsUsed)
    {
        int bidValue = setValue + coinsUsed;
        if (PlayerManager.instance.activePlayerInfo == PlayerManager.LocalPlayerInfo &&
            bidValue > highestBid)
        {
            PlayerManager.LocalPlayerInfo.SetNSyncCoinsUsed(coinsUsed);
            instance.view.RPC("NewHighestBid", RpcTarget.All, bidValue, PlayerManager.LocalPlayerInfo.view.ViewID);
            foreach(CardNetworked cardSubmitted in instance.bidCardSet)
            {
                cardSubmitted.isSubmitted = true;
            }
            PlayerManager.instance.NextPlayer();
        }

    }
    [PunRPC]
    public void NewHighestBid(int bidValue, int playerViewID)
    {
        highestBid = bidValue;
        PlayerInfo biddingPlayer =PhotonView.Find(playerViewID).GetComponent<PlayerInfo>();
        biddingPlayer.bid = bidValue;
        biddingPlayer.placedBid = true;
        biddingPlayer.UpdateInfo();
    }

    public void DrawNewCards()
    {
        for (int i = 0; i < 3; i++)
        {
            deck.DrawAndInstantiateCard(UIHandler);
        }
    }

    public override void EndOfTurnPUNEventHandler(PlayerInfo winner)
    {
        base.EndOfTurnPUNEventHandler(winner);
        highestBid = -1;
        localPlayerPotentialBidValue = -1;
        bidCardSet = new List<CardNetworked>();
        BidTool.Reset();
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(WaitNDraw());
        }
    }
    private IEnumerator WaitNDraw()
    {
        yield return new WaitForSeconds(1f);
        DrawNewCards();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class PlayerInfo : PunEventHandler
{
    public int controllingUser = -1, bid = 0;
    private int cardsInHand = 0, coins = 12;
    public int coinsUsed = 0;
    public int CardsInHand
    {
        get { return cardsInHand; }
        set { 
            cardsInHand = value;
            UpdateInfo();
            }
    }
    public int Coins
    {
        get { return coins; }
        set { 
            coins = value;
            UpdateInfo();
        }
    }
    public bool folded = false;
    public bool placedBid = false;
    public string playerName = "Waiting For A Player";
    public TextMeshProUGUI uiElement;
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        PlayerManager.players.Add(this);
        this.transform.parent = PlayerManager.newPlayerInfoParent.transform;
        this.transform.position = new Vector3(100, 200, 0);
    }
    void Start()
    {
        EventTriggerInterface.AddEventTriggerStatic(EventTriggerInterface.supportedEvents.OnMouseZeroDown,
            TakeControl,
            this.gameObject);
        gameObject.AddComponent<DragableButton>();
    }
    public override void EndOfTurnPUNEventHandler(PlayerInfo winner)
    {
        Messages.DisplayMessage("EoTEventOnPlayerInfoTriggered " + playerName);
        base.EndOfTurnPUNEventHandler(winner);
        bid = 0;
        coinsUsed = 0;
        folded = false;
        placedBid = false;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        uiElement.text = playerName + "\n" +
            cardsInHand.ToString() + " Cards in Hand\n" +
            coins.ToString() + " Coins\n";
        if (folded)
        {
            uiElement.text += "Folded";
        }else if(placedBid)
        {
            uiElement.text += "Bids for " + bid.ToString();
        }
        else
        {
            uiElement.text += "No Bid yet";
        }
    }

    public void TakeControl()
    {
        bool controlsOtherPlayer = false;
        foreach( PlayerInfo player in PlayerManager.players)
        {
            if (player.controllingUser == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                controlsOtherPlayer = true;
            }
        }
        if (!controlsOtherPlayer)
        {
            view.RPC("TakeControlRPC",
                RpcTarget.All,
                PhotonNetwork.LocalPlayer.ActorNumber,
                PlayerManager.instance.nameInputField.text); 
        }
    }
    [PunRPC]
    public void TakeControlRPC(int playerTakingControl, string playerNameArg)
    {
        controllingUser = playerTakingControl;
        playerName = playerNameArg;
        UpdateInfo();
    }

    [PunRPC]
    public void RemoveRPC()
    {
        PlayerManager.players.Remove(this);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    public void SetNSyncCoinsUsed(int value)
    {
        coinsUsed = value;
        view.RPC("SetCoinsUsedRPC", RpcTarget.Others, value);
    }
    [PunRPC]
    private void SetCoinsUsedRPC(int value)
    {
        coinsUsed = value;
    }
    public void SetNSyncCoins(int value)
    {
        Coins = value;
        view.RPC("SetCoinsRPC", RpcTarget.Others, value);
    }

    [PunRPC]
    private void SetCoinsRPC(int value)
    {
        Coins = value;
    }
}

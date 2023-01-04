using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : NetworkedItem
{
    public static PlayerManager instance;
    public static GameObject newPlayerInfoParent;
    public static List<PlayerInfo> players;
    public GameObject addPlayerButton, removePlayerButton;
    public TMP_InputField nameInputField;
    private PlayerInfo localPlayerInfo;
    public PlayerInfo activePlayerInfo = null;

    public float lastBlinkTime = 0f;
    public static PlayerInfo LocalPlayerInfo {
        get 
            {
            instance.localPlayerInfo = null;
            if (players.Count > 0)
                {
                    foreach (PlayerInfo player in players)
                    {
                        if (player.controllingUser == PhotonNetwork.LocalPlayer.ActorNumber)
                        {
                        instance.localPlayerInfo = player;
                        }
                    }
                }
                return instance.localPlayerInfo;
            }
        }
    private void Start()
    {
        InitPhotonViewReferences();
        players = new List<PlayerInfo>();
        instance = this;
        newPlayerInfoParent = this.gameObject;

        EventTriggerInterface.AddEventTriggerStatic(EventTriggerInterface.supportedEvents.OnMouseZeroDown,
            AddPlayer,
            addPlayerButton);
        EventTriggerInterface.AddEventTriggerStatic(EventTriggerInterface.supportedEvents.OnMouseZeroDown,
            RemovePlayer,
            removePlayerButton);
    }
    private void Update()
    {
        if(GameLogicNetworked.instance.isInitialised && lastBlinkTime < Time.time - 1.4f)
        {
            lastBlinkTime = Time.time;
            IEnumerator coroutine = ShowActivePlayer(activePlayerInfo.gameObject);
            StartCoroutine(coroutine);
        }
    }

    public IEnumerator ShowActivePlayer(GameObject activePlayerObject)
    {
        PlayerInfo playerInfo = activePlayerObject.GetComponent<PlayerInfo>();    
        string tempName = playerInfo.playerName;
        playerInfo.playerName += "+++";
        playerInfo.UpdateInfo();
        yield return new WaitForSeconds(0.3f);
        playerInfo.playerName = tempName;
        playerInfo.UpdateInfo();
    }

    public void AddPlayer()
    {
        GameObject newPlayer = PhotonNetwork.Instantiate("playerInfo", Vector3.zero, Quaternion.identity);
    }
    public void RemovePlayer()
    {
        PlayerInfo oneToRemove = null;
        if (players == null || players.Count == 0)
        {
            return;
        }
        foreach( PlayerInfo player in players)
        {
            if(player.controllingUser == -1)
            {
                oneToRemove = player;
            }
        }
        if(oneToRemove != null)
        {
            players.Remove(players[players.Count - 1]);
            PhotonNetwork.Destroy(oneToRemove.gameObject);
        }
    }
    public void NextPlayer()
    {
        if(activePlayerInfo == null)
        {
            Messages.DisplayMessage("Randomly selecting a new player");
            int randomValue = Random.Range(0, players.Count);
            SetActivePlayer(players[randomValue].punID);
        }
        else
        {
            int i = 0;
            bool newPlayerSelected = false;
            foreach(PlayerInfo player in players)
            {
                i++;
                if(player == activePlayerInfo && !newPlayerSelected)
                {
                    i %= players.Count;
                    if (players[i].folded == false)
                    {
                        newPlayerSelected = true;
                        SetActivePlayer(players[i].punID);
                    }
                    else
                    {
                        Messages.DisplayMessage("ERROR");
                    }
                }
            }
            if(newPlayerSelected == false)
            {
                Messages.DisplayMessage("selecting a new player failure");
            }
        }
    }

    public void SetActivePlayer(int playerID)
    {
        view.RPC("SetActivePlayerRPC", RpcTarget.All, playerID);
    }
    [PunRPC]
    public void SetActivePlayerRPC(int playerID)
    {
        foreach (PlayerInfo player in players)
        {
            if (player.punID == playerID)
            {
                activePlayerInfo = player;
            }
        }
    }
}

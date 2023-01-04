using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class CardNetworked : PunEventHandler
{
    private bool isSelected = false;
    public bool IsSelected
    {
        get { return isSelected; }
        set 
        { 
            isSelected = value;
            UpdateSelectUI();
        }
    }
    public bool isSubmitted = false;
    private PlayerInfo controllingPlayer = null;
    public PlayerInfo ControllingPlayer
    {
        get { return controllingPlayer; }
        set { 
            controllingPlayer = value;
            this.transform.parent = CardSpawner.instance.controlledCardsParent.transform;
            }
    }
    public ColorPalet colorPalet;
    public TextMeshProUGUI mainValue, bonus;
    public Button background, selectButton;
    public TestCard cardSO;
    public GameLogicNetworked logic;

    public string testValue;

    public override void OnEnable()
    {
        base.OnEnable();
        this.gameObject.AddComponent<DragableButton>();
    }

    public override void EndOfTurnPunEventHandler(PlayerInfo winner)
    {
        base.EndOfTurnPunEventHandler(winner);
        if (winner == PlayerManager.LocalPlayerInfo && isSubmitted)
        {
            //PhotonNetwork.Destroy(this.gameObject);
            view.RPC("RPCDestroyCard", RpcTarget.All);
        }else if(controllingPlayer == null)
        {
            ControllingPlayer = winner;
            UpdateSelectUI();
            if (winner != PlayerManager.LocalPlayerInfo)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                this.view.TransferOwnership(PhotonNetwork.LocalPlayer);
                this.transform.parent = CardSpawner.instance.controlledCardsParent.transform;
            }
        }
        else
        {
            IsSelected = false;
            isSubmitted = false;
        }
    }
    [PunRPC]
    public void RPCDestroyCard()
    {
        if (this.view.Controller == PhotonNetwork.LocalPlayer)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        EventTriggerInterface ETReference = selectButton.gameObject.AddComponent<EventTriggerInterface>();
        ETReference.AddEventTrigger(EventTriggerInterface.supportedEvents.OnMouseZeroDown, SelectButtonPressed);
    }

    private void SelectButtonPressed()
    {
        if (isSubmitted || ControllingPlayer != PlayerManager.LocalPlayerInfo)
        {
            return;
        }
        IsSelected = !IsSelected;
        logic.AddOrRemoveCardFromSet(this, isSelected);

    }

    [PunRPC]
    public void InitCardRPC(int spawnerPunId, int cardTypeID)
    {
        logic = GameLogicNetworked.instance;
        cardSO = logic.GetComponent<CardIdentifier>().FindWithID(cardTypeID);
        //logic.GetComponent<CardSpawner>().SetCardsPosition(this.gameObject);
        this.transform.parent = CardSpawner.instance.spawnParent.transform;
        mainValue.text = cardSO.mainValue.ToString();
        bonus.text = cardSO.bonus.ToString();

        background.colors = FastColorBlock(colorPalet.GetColor((ColorPalet.coloursEnum)cardSO.color), background);
    }
    private ColorBlock FastColorBlock(Color color, Button button)
    {
        ColorBlock tempColourBlock = button.colors;
        tempColourBlock.normalColor = color;
        tempColourBlock.highlightedColor = color;
        tempColourBlock.selectedColor = color;
        return tempColourBlock;
    }
    private void UpdateSelectUI()
    {
        if (isSelected)
        {
            selectButton.colors = FastColorBlock(colorPalet.GetColor(ColorPalet.coloursEnum.selected), selectButton);
        }
        else
        {
            selectButton.colors = FastColorBlock(colorPalet.GetColor(ColorPalet.coloursEnum.owned), selectButton);
        }
    }
}

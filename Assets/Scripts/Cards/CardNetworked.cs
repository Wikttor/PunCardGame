using UnityEngine;
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
            EndOfTurnUpdateSelectUI();
        }
    }
    public bool isSubmitted = false;
    private PlayerInfo controllingPlayer = null;
    public PlayerInfo ControllingPlayer
    {
        get { return controllingPlayer; }
        set
        {
            controllingPlayer = value;
            this.transform.parent = CardSpawner.instance.controlledCardsParent.transform;
        }
    }
    public ColorPalet colorPalet;
    public TextMeshProUGUI mainValue, bonus;
    public Button background, selectButton;
    public TestCard cardScriptableObject;
    public GameLogicNetworked logic;

    public override void OnEnable()
    {
        base.OnEnable();
        this.gameObject.AddComponent<DragableButton>();
    }

    public override void EndOfTurnPUNEventHandler(PlayerInfo winner)
    {
        base.EndOfTurnPUNEventHandler(winner);
        if (winner == PlayerManager.LocalPlayerInfo && isSubmitted)
        {
            view.RPC("RPCDestroyCard", RpcTarget.All);
        }
        else if (controllingPlayer == null)
        {
            ControllingPlayer = winner;
            EndOfTurnUpdateSelectUI();
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
        EventTriggerInterface EventTrigger = selectButton.gameObject.AddComponent<EventTriggerInterface>();
        EventTrigger.AddEventTrigger(EventTriggerInterface.supportedEvents.OnMouseZeroDown, OnSelectButtonPressed);
    }

    private void OnSelectButtonPressed()
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
        cardScriptableObject = logic.GetComponent<CardIdentifier>().FindByID(cardTypeID);
        this.transform.parent = CardSpawner.instance.spawnParent.transform;
        mainValue.text = cardScriptableObject.mainValue.ToString();
        bonus.text = cardScriptableObject.bonus.ToString();

        background.colors = FastColorBlock(colorPalet.GetColor((ColorPalet.colorsEnum)cardScriptableObject.color), background);
    }
    private ColorBlock FastColorBlock(Color color, Button button)
    {
        ColorBlock tempColorBlock = button.colors;
        tempColorBlock.normalColor = color;
        tempColorBlock.highlightedColor = color;
        tempColorBlock.selectedColor = color;
        return tempColorBlock;
    }
    private void EndOfTurnUpdateSelectUI()
    {
        ColorPalet.colorsEnum color = isSelected ? ColorPalet.colorsEnum.selected : ColorPalet.colorsEnum.owned;
        selectButton.colors = FastColorBlock(colorPalet.GetColor(color), selectButton);
    }
}

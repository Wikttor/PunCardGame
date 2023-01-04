using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BidTool : MonoBehaviour
{
    public static BidTool instance;
    public static int setValue = 0, coinsUsed = 0;
    public static int SetValue
    {
        get { return setValue; }
        set 
        { 
            setValue = value;
            UpdateBidIUI();
        }
    }
    public TextMeshProUGUI text;

    private void Start()
    {
        instance = this;
    }

    public static void AddCoin()
    {
        if (coinsUsed < PlayerManager.LocalPlayerInfo.Coins)
        {
            coinsUsed++;
            UpdateBidIUI();
        }
    }
    public static void RemoveCoin()
    {
        if (coinsUsed >0 && PlayerManager.LocalPlayerInfo.bid < coinsUsed + setValue)
        {
            coinsUsed--;
            UpdateBidIUI();
        }
    }

    public static void UpdateBidIUI()
    {
        instance.text.text = (setValue + coinsUsed).ToString();
        if(coinsUsed > 0)
        {
            instance.text.text += "(" + coinsUsed.ToString() + " coins)";
        }
    }

    public static void Fold()
    {
        Messages.DisplayMessage("Debug: Fold PRessed");
        if(PlayerManager.instance.activePlayerInfo == PlayerManager.LocalPlayerInfo)
        {
            GameLogicNetworked.instance.PlayerFolded();
        }
        else
        {
            Messages.DisplayMessage("cannot fold, its not your turn");
        }
    }
    public static void TryBid()
    {
        GameLogicNetworked.TryBid(setValue , coinsUsed);
    }
    public static void Reset()
    {
        setValue = 0;
        coinsUsed = 0;
        UpdateBidIUI();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class MyPunEvents
{
    public enum EventCodes { EoT };

    public static void FastEvent(EventCodes code)
    {
        object[] emptyContent = new object[0];
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)code, emptyContent, raiseEventOptions, SendOptions.SendReliable);
    }

    public static void RaiseEndOfTurn(PlayerInfo winner)
    {
        object[] content = new object[1];
        content[0] = winner.view.ViewID;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)EventCodes.EoT, content, raiseEventOptions, SendOptions.SendReliable);
    }
}

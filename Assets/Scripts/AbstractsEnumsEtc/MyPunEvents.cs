using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class MyPunEvents
{
    public enum EventCodes { EoT };

    public static void RaiseEndOfTurn(PlayerInfo winner)
    {
        object[] content = new object[1];
        content[0] = winner.view.ViewID;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)EventCodes.EoT, content, raiseEventOptions, SendOptions.SendReliable);
    }
}

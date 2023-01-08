using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class PunEventHandler : NetworkedItem, IOnEventCallback
{

    public virtual void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public virtual void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public virtual void OnEvent(EventData eventData)
    {
        if (eventData.Code == (byte)MyPunEvents.EventCodes.EoT)
        {
            object[] data = (object[])eventData.CustomData;
            int winnerID = (int)data[0];
            EndOfTurnPUNEventHandler(PhotonView.Find(winnerID).GetComponent<PlayerInfo>());
        }
    }

    public virtual void EndOfTurnPUNEventHandler(PlayerInfo winner)
    {
    }
}

using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MyPUNBasics : MonoBehaviourPunCallbacks
{
    private const string defaultRoom = "default_room";
    public static bool joinedRoom = false;
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Messages.PUNConnectedSuccessfully();
        PhotonNetwork.CreateRoom(defaultRoom);
        StartCoroutine(JoinRoomCreatedByOtherPlayer());
    }
    public override void OnJoinedRoom()
    {
        Messages.PUNJoinedRoom();
        joinedRoom = true;
    }

    public override void OnCreatedRoom()
    {
        Messages.PUNCreatedRoom();
    }

    public IEnumerator JoinRoomCreatedByOtherPlayer()//TODO replace with a proper callback override
    {
        yield return new WaitForSeconds(3f);
        if (!joinedRoom)
        {
            PhotonNetwork.JoinRoom(defaultRoom);
        }
    }
}

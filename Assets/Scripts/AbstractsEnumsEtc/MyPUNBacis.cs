using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyPUNBacis : MonoBehaviourPunCallbacks
{
    private const string defaultRoom = "default_room";
    public static bool joinedRoom = false;
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(DoublecheckIfJoinedRoom());
    }

    IEnumerator DoublecheckIfJoinedRoom()
    {
        bool isJobDone = false;
        while (!isJobDone)
        {
            if (joinedRoom)
            {
                isJobDone = true;
            }
            yield return new WaitForSeconds(0.2f);
        }
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

    public IEnumerator JoinRoomCreatedByOtherPlayer()
    {
        yield return new WaitForSeconds(3f);
        if (!joinedRoom)
        {
            PhotonNetwork.JoinRoom(defaultRoom);
        }
    }
}

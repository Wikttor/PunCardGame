using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class NetworkedItem : MonoBehaviour, IPunInstantiateMagicCallback
{
    public int punID;
    public PhotonView view;

    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        view = info.photonView;
        punID = (int)view.ViewID;
    }

    public void InitPhotonViewReferences()
    {
        view = this.GetComponent<PhotonView>();
        punID = (int)view.ViewID;
    }
}

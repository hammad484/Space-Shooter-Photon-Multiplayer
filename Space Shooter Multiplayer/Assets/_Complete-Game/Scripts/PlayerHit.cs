using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerHit : MonoBehaviour
{
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
 // private void OnTriggerEnter(Collider other)
 // {
 //     if (other.gameObject.CompareTag("Player"))
 //     {
 //        
 //         if (photonView.IsMine)
 //        {
 //             Debug.LogError("Bullet hit to Player from " + this.name);
 //             Debug.LogError("Player destroyeddddddd");
 //             other.gameObject.GetComponent<PhotonView>().RPC("DestroySpaceship", RpcTarget.All);
 //             PhotonNetwork.Destroy(other.gameObject);
 //
 //         }
 //     }
 // }

    private void DestroyAsteroidLocally()
    {
        GetComponent<Renderer>().enabled = false;
    }


 //  private void DestroyAsteroidGlobally(GameObject object)
 //  {
 //      PhotonNetwork.Destroy(gameObject);
 //  }
}

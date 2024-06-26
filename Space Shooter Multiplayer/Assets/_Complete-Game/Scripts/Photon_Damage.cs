using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photon_Damage : MonoBehaviour
{
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                Debug.LogError("Player destroyeddddddd");
                other.gameObject.GetComponent<PhotonView>().RPC("DestroySpaceship", RpcTarget.All);

                DestroyAsteroidGlobally();
            }
        //  else
        //  {
        //      DestroyAsteroidLocally();
        //  }
        }
    }

 //  private void DestroyAsteroidLocally()
 //  {
 //      GetComponent<Renderer>().enabled = false;
 //  }


    private void DestroyAsteroidGlobally()
    {
        PhotonNetwork.Destroy(gameObject);
    }

}

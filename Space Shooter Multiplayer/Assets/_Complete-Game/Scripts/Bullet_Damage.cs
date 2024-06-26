using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

namespace Photon.Pun.UtilityScripts
{
    public class Bullet_Damage : MonoBehaviour
    {
        public GameObject explosion;
        public GameObject playerExplosion;
        public int scoreValue;
        private Done_GameController gameController;

        private PhotonView photonView;

        void Start()
        {
            photonView = GetComponent<PhotonView>();
            GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
            if (gameControllerObject != null)
            {
                gameController = gameControllerObject.GetComponent<Done_GameController>();
            }
            if (gameController == null)
            {
                Debug.Log("Cannot find 'GameController' script");
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Boundary" || other.tag == "Enemy")
            {
                return;
            }

            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

     //   if (other.gameObject.CompareTag("Bullet"))
     //   {
     //       if (photonView.IsMine)
     //       {
     //           photon_bullet bullet = other.gameObject.GetComponent<photon_bullet>();
     //           bullet.Owner.AddScore(2);
     //
     //           DestroyAsteroidGlobally();
     //       }
     //       else
     //       {
     //           DestroyAsteroidLocally();
     //       }
     //   }

            if (other.gameObject.CompareTag("Player"))
            {
                if (photonView.IsMine)
                {
                    Debug.LogError("Player destroyeddddddd");
                    other.gameObject.GetComponent<PhotonView>().RPC("DestroySpaceship", RpcTarget.All);

                    DestroyAsteroidGlobally();
                }
            }

      //      gameController.AddScore(scoreValue);
      //      Destroy(other.gameObject);
      //      Destroy(gameObject);
        }
        /*
          void OnTriggerEnter(Collider collision)
         {

             if (collision.gameObject.CompareTag("Bullet"))
             {
                 if (photonView.IsMine)
                 {
                     photon_bullet bullet = collision.gameObject.GetComponent<photon_bullet>();
                     bullet.Owner.AddScore(2);

                     DestroyAsteroidGlobally();
                 }
                 else
                 {
                     DestroyAsteroidLocally();
                 }
             }
             else if (collision.gameObject.CompareTag("Player"))
             {
                 if (photonView.IsMine)
                 {
                     collision.gameObject.GetComponent<PhotonView>().RPC("DestroySpaceship", RpcTarget.All);

                     DestroyAsteroidGlobally();
                 }
             }
         }
        */
        private void DestroyAsteroidLocally()
        {
            GetComponent<Renderer>().enabled = false;
        }


     private void DestroyAsteroidGlobally()
     {
         PhotonNetwork.Destroy(gameObject);
     }















    }
}
using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
    private float deltaY, deltaX;
    public float speed;
	public float tilt;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	 
	private float nextFire;

    private PhotonView photonView;
    private bool controllable = true;
    private new Rigidbody rigidbody;
    private float rotation = 0.0f;
    private float acceleration = 0.0f;
    private new Collider collider;
    private new Renderer renderer;
    public TextMeshProUGUI playername;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }
    private void Start()
    {
        Debug.LogError(" Player Name : " + photonView.Owner.NickName);
        playername.text = photonView.Owner.NickName;
    }

    void Update()
    {
        if (!photonView.AmOwner || !controllable)
        {
            return;
        }

        // we don't want the master client to apply input to remote ships while the remote player is inactive
        if (this.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //	Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            photonView.RPC("Fire", RpcTarget.AllViaServer, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    deltaX = touchPos.x - transform.position.x;
                    deltaY = touchPos.y - transform.position.y;
                    break;

                case TouchPhase.Moved:
                    rigidbody.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                    break;

                case TouchPhase.Ended:
                    rigidbody.velocity = Vector2.zero;
                    break;
            }
        }
    }

    void FixedUpdate ()
	{
        if (!photonView.IsMine)
        {
            return;
        }

        if (!controllable)
        {
            return;
        }
    //    float moveHorizontal = Input.GetAxis ("Horizontal");
	//	float moveVertical = Input.GetAxis ("Vertical");

	//	Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
	//	GetComponent<Rigidbody>().velocity = movement * speed;
		

		GetComponent<Rigidbody>().position = new Vector3
		( 
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
		
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}
    private IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(Database.PLAYER_RESPAWN_TIME);

        photonView.RPC("RespawnSpaceship", RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void DestroySpaceship()
    {
        transform.position = new Vector3 (-10, 0, 0);
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        collider.enabled = false;
        renderer.enabled = false;

        controllable = false;
        controllable = false;


        if (photonView.IsMine)
        {
            object lives;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Database.PLAYER_LIVES, out lives))
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { Database.PLAYER_LIVES, ((int)lives <= 1) ? 0 : ((int)lives - 1) } });

                if (((int)lives) > 1)
                {
                    StartCoroutine("WaitForRespawn");
                }
            }
        }
    }
    [PunRPC]
    public void Fire(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject bullet;

        /** Use this if you want to fire one bullet at a time **/
        bullet = Instantiate(shot, position, Quaternion.identity) as GameObject;
        bullet.GetComponent<photon_bullet>().InitializeBullet(photonView.Owner, (rotation * Vector3.forward), Mathf.Abs(lag));


        /** Use this if you want to fire two bullets at once **/
        //Vector3 baseX = rotation * Vector3.right;
        //Vector3 baseZ = rotation * Vector3.forward;

        //Vector3 offsetLeft = -1.5f * baseX - 0.5f * baseZ;
        //Vector3 offsetRight = 1.5f * baseX - 0.5f * baseZ;

        //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetLeft, Quaternion.identity) as GameObject;
        //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
        //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetRight, Quaternion.identity) as GameObject;
        //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
    }
    [PunRPC]
    public void RespawnSpaceship()
    {
         renderer.enabled = true;
        collider.enabled = true;
        controllable = true;
        transform.position = new Vector3(0, 0, 0);
     
    }

  

}

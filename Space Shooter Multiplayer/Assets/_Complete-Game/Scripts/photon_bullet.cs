using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photon_bullet : MonoBehaviour
{
    public Player Owner { get; private set; }

    public void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    public void InitializeBullet(Player owner, Vector3 originalDirection, float lag)
    {
        Owner = owner;

        transform.forward = originalDirection;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = originalDirection * 20f;
        rigidbody.position += rigidbody.velocity * lag;
    }
}

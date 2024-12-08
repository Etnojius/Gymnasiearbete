using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CancelMagic : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            transform.localScale += 50 * Time.deltaTime * Vector3.one;
            if (transform.localScale.x >= 200)
            {
                NetworkObject.Despawn();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            if (collision.gameObject.GetComponent<BaseSpell>() != null)
            {
                collision.gameObject.GetComponent<NetworkObject>().Despawn();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BaseSpell>() != null)
        {
            other.gameObject.GetComponent<NetworkObject>().Despawn();
        }
        if (other.gameObject.GetComponent<NetworkPlayer>() != null)
        {
            other.gameObject.GetComponent<NetworkPlayer>().DisableMagic();
        }
    }
}

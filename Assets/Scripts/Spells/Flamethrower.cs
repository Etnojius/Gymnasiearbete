using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Flamethrower : NetworkBehaviour
{
    public float damagePerSecond = 15f;
    public NetworkVariable<ulong> ownerId;
    Transform target;
    List<NetworkPlayer> playersInFire = new List<NetworkPlayer>();
    // Start is called before the first frame update
    void Start()
    {
        var targetList = GameObject.FindGameObjectsWithTag("Player");
        foreach (var potentialTarget in targetList)
        {
            if (potentialTarget.GetComponent<NetworkObject>().NetworkObjectId == ownerId.Value)
            {
                target = potentialTarget.GetComponent<NetworkPlayer>().rightHand;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;

        if (IsServer)
        {
            foreach(NetworkPlayer player in playersInFire)
            {
                player.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.tag == "Player")
            {
                var instance = other.GetComponent<NetworkPlayer>();
                if (instance.NetworkObjectId != ownerId.Value)
                {
                    playersInFire.Add(instance);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.tag == "Player")
            {
                var instance = other.GetComponent<NetworkPlayer>();
                if (playersInFire.Contains(instance))
                {
                    playersInFire.Remove(instance);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Flamethrower : BaseSpell
{
    public float damagePerSecond = 15f;
    Transform target;
    List<NetworkPlayer> playersInFire = new List<NetworkPlayer>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
        else
        {
            FindTarget();
        }

        if (IsServer)
        {
            foreach(NetworkPlayer player in playersInFire)
            {
                player.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }

    private void FindTarget()
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

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Player"))
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
            if (other.gameObject.CompareTag("Player"))
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

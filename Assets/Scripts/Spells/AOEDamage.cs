using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : BaseSpell
{
    public float damagePerSecond = 15f;
    protected List<NetworkPlayer> playersInAOE = new List<NetworkPlayer>();

    // Update is called once per frame
    protected virtual void Update()
    {
        DealDamage();   
    }

    protected virtual void DealDamage()
    {
        foreach (NetworkPlayer player in playersInAOE)
        {
            player.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var instance = other.GetComponent<NetworkPlayer>();
                if (instance.NetworkObjectId != ownerId.Value)
                {
                    playersInAOE.Add(instance);
                }
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var instance = other.GetComponent<NetworkPlayer>();
                if (playersInAOE.Contains(instance))
                {
                    playersInAOE.Remove(instance);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Flamethrower : AOEDamage
{
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
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
            DealDamage();
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

   

   
}

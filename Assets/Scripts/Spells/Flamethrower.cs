using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Flamethrower : NetworkBehaviour
{
    public NetworkVariable<ulong> ownerId;
    Transform target;
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
    }
}

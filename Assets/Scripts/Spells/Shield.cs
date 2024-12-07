using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Shield : NetworkBehaviour
{
    public ulong ownerId;
    public GameObject owner;
    public float duration = 0.5f;
    public bool isParry = true;
    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {
            var targetList = GameObject.FindGameObjectsWithTag("Player");
            foreach (var potentialTarget in targetList)
            {
                if (potentialTarget.GetComponent<NetworkObject>().NetworkObjectId == ownerId)
                {
                    owner = potentialTarget;
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            transform.position = owner.transform.position;
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                NetworkObject.Despawn();
            }
        }
    }
}

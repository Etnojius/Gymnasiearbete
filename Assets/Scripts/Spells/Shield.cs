using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Shield : BaseSpell
{
    public GameObject owner;
    public bool isParry = true;
    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {
            var targetList = GameObject.FindGameObjectsWithTag("Player");
            foreach (var potentialTarget in targetList)
            {
                if (potentialTarget.GetComponent<NetworkObject>().NetworkObjectId == ownerId.Value)
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
        }
    }
}

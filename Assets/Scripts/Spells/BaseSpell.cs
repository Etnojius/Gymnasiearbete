using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseSpell : NetworkBehaviour
{
    public NetworkVariable<ulong> ownerId;
    public float duration = 30f;

    protected virtual void FixedUpdate()
    {
        if (IsServer)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                DespawnSelf();
            }
        }
    }

    protected virtual void DespawnSelf()
    {
        NetworkObject.Despawn(true);
    }
}

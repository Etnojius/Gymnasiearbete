using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public float hp = 100;
    public float maxHP = 100;

    public Renderer[] meshToDisable;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            foreach (var item in meshToDisable)
            {
                item.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            head.position = VRRigReferences.Instance.head.position;
            head.rotation = VRRigReferences.Instance.head.rotation;

            leftHand.position = VRRigReferences.Instance.leftHand.position;
            leftHand.rotation = VRRigReferences.Instance.leftHand.rotation;

            rightHand.position = VRRigReferences.Instance.rightHand.position;
            rightHand.rotation = VRRigReferences.Instance.rightHand.rotation;
        }

        if (IsServer)
        {
            if (hp <= 0)
            {
                DeathRPC();
                hp = maxHP;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        TakeDamageRPC();
    }

    [Rpc(SendTo.Owner)]
    private void TakeDamageRPC()
    {
        InputManager.Instance.VibrateController(true, true);
    }

    [Rpc(SendTo.Owner)]
    private void DeathRPC()
    {
        InputTracker.Instance.transform.position = new Vector3(0, 0, 1000);
        InputManager.Instance.VibrateController(true, true);
    }
}

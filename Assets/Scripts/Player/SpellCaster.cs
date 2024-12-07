using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpellCaster : NetworkBehaviour
{
    private float castingTimingLeniency = 0.5f;
    public bool canCast = true;
    private bool castingMagicBolt = false;

    private float magicBoltCastingTime = 0.5f;

    public GameObject magicBolt;
    public GameObject shield;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            InputTracker.Instance.spellCaster = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            if (castingMagicBolt)
            {
                if (!InputManager.Instance.rightGrip)
                {
                    CastMagicBoltRPC(InputManager.Instance.rightHandTransform.position, InputManager.Instance.rightHandDirection);
                    castingMagicBolt = false;
                    canCast = true;
                }
            }
        }
    }

    public void CastSpell(InputState input)
    {
        if (canCast)
        {
            if (input.rightZone == 3 && input.rightGrip && input.rightZoneDuration >= magicBoltCastingTime && input.rightGripDuration >= magicBoltCastingTime)
            {
                InputManager.Instance.VibrateController(true, false);
                InputTracker.Instance.ResetInputState();
                castingMagicBolt = true;
                canCast = false;
            }
            else if (input.rightZone == 3 && input.prevRightZone == 2 && input.prevLeftZone == 2 && input.leftZone == 1 && input.rightGrip && input.leftGrip && input.rightZoneDuration <= castingTimingLeniency && input.leftZoneDuration <= castingTimingLeniency && input.leftGripDuration >= input.leftZoneDuration && input.rightGripDuration >= input.rightZoneDuration)
            {
                InputManager.Instance.VibrateController(true, true);
                InputTracker.Instance.ResetInputState();
                CastShieldRPC();
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void CastMagicBoltRPC(Vector3 position, Vector3 direction)
    {
        var instance = Instantiate(magicBolt).GetComponent<BaseProjectile>();
        instance.direction = direction;
        instance.transform.position = position;
        instance.GetComponent<NetworkObject>().Spawn();
        instance.ownerId = GetComponent<NetworkObject>().NetworkObjectId;
    }

    [Rpc(SendTo.Server)]
    private void CastShieldRPC()
    {
        var instance = Instantiate(shield).GetComponent<Shield>();
        instance.ownerId = GetComponent<NetworkObject>().NetworkObjectId;
        instance.GetComponent<NetworkObject>().Spawn();
    }
}

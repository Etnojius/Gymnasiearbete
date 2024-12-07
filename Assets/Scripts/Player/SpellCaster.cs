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
    public GameObject flamethrower;
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
            if (input.leftTrigger && input.leftGrip && input.rightTrigger && input.rightGrip && input.leftZone == 1 && input.rightZone == 6 && input.prevLeftZone == 4 && input.prevRightZone == 3 && input.rightZoneDuration <= castingTimingLeniency && input.leftZoneDuration <= castingTimingLeniency && input.leftGripDuration >= input.leftZoneDuration && input.leftTriggerDuration >= input.leftZoneDuration && input.rightTriggerDuration >= input.rightZoneDuration && input.rightGripDuration >= input.rightZoneDuration)
            {
                CastMagicCircleRPC(1);
                InputTracker.Instance.ResetInputState();
            }
            else if (input.leftTrigger && input.leftGrip && input.rightTrigger && input.rightGrip && input.leftZone == 5 && input.rightZone == 5 && input.prevLeftZone == 4 && input.prevRightZone == 6 && input.rightZoneDuration <= castingTimingLeniency && input.leftZoneDuration <= castingTimingLeniency && input.leftGripDuration >= input.leftZoneDuration && input.leftTriggerDuration >= input.leftZoneDuration && input.rightTriggerDuration >= input.rightZoneDuration && input.rightGripDuration >= input.rightZoneDuration)
            {
                CastMagicCircleRPC(2);
                InputTracker.Instance.ResetInputState();
            }
            else if (input.leftTrigger && input.leftGrip && input.rightTrigger && input.rightGrip && input.leftZone == 3 && input.rightZone == 1 && input.prevLeftZone == 2 && input.prevRightZone == 2 && input.rightZoneDuration <= castingTimingLeniency && input.leftZoneDuration <= castingTimingLeniency && input.leftGripDuration >= input.leftZoneDuration && input.leftTriggerDuration >= input.leftZoneDuration && input.rightTriggerDuration >= input.rightZoneDuration && input.rightGripDuration >= input.rightZoneDuration)
            {
                CastMagicCircleRPC(3);
                InputTracker.Instance.ResetInputState();
            }
            else if (input.innerCircle == 2 && input.middleCircle == 1 && input.rightZone == 3 && input.prevRightZone == 6 && input.rightTrigger && input.rightZoneDuration <= input.rightGripDuration)
            {
                CastFlamethrowerRPC();
                ConsumeMagicCirclesRPC();
            }
            else if (input.rightZone == 3 && input.rightGrip && input.rightZoneDuration >= magicBoltCastingTime && input.rightGripDuration >= magicBoltCastingTime && input.rightZoneDuration >= input.rightGripDuration)
            {
                InputManager.Instance.VibrateController(true, false);
                InputTracker.Instance.ResetInputState();
                castingMagicBolt = true;
                canCast = false;
            }
            else if (input.rightZone == 3 && input.prevRightZone == 2 && input.prevLeftZone == 2 && input.leftZone == 1 && input.rightGrip && input.leftGrip && input.rightZoneDuration <= castingTimingLeniency && input.leftZoneDuration <= castingTimingLeniency && input.leftGripDuration >= input.leftZoneDuration && input.rightGripDuration >= input.rightZoneDuration)
            {
                InputTracker.Instance.ResetInputState();
                CastShieldRPC(transform.position);
            }
            else if (input.yButton || input.bButton)
            {
                CastMagicCircleRPC(0);
                InputTracker.Instance.ResetInputState();
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void ConsumeMagicCirclesRPC()
    {
        var networkPlayer = GetComponent<NetworkPlayer>();
        networkPlayer.innerCircle.Value = 0;
        networkPlayer.middleCircle.Value = 0;
        networkPlayer.outerCircle.Value = 0;
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
    private void CastShieldRPC(Vector3 position)
    {
        var instance = Instantiate(shield).GetComponent<Shield>();
        instance.transform.position = position;
        instance.GetComponent<NetworkObject>().Spawn();
        instance.ownerId.Value = GetComponent<NetworkObject>().NetworkObjectId;
    }

    [Rpc(SendTo.Server)]
    private void CastFlamethrowerRPC()
    {
        var instance = Instantiate(flamethrower).GetComponent<Flamethrower>();
        instance.GetComponent<NetworkObject>().Spawn();
        instance.ownerId.Value = GetComponent<NetworkObject>().NetworkObjectId;
    }

    [Rpc(SendTo.Server)]
    private void CastMagicCircleRPC(byte index)
    {
        var networkPlayer = GetComponent<NetworkPlayer>();
        if (index == 0)
        {
            networkPlayer.innerCircle.Value = 0;
            networkPlayer.middleCircle.Value = 0;
            networkPlayer.outerCircle.Value = 0;
        }
        else
        {
            if (networkPlayer.innerCircle.Value == 0)
            {
                networkPlayer.innerCircle.Value = index;
            }
            else if (networkPlayer.middleCircle.Value == 0)
            {
                networkPlayer.middleCircle.Value = index;
            }
            else if (networkPlayer.outerCircle.Value == 0)
            {
                networkPlayer.outerCircle.Value = index;
            }
        }
    }
}

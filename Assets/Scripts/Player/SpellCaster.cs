using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpellCaster : NetworkBehaviour
{
    public const byte redCircle = 1;
    public const byte yellowCircle = 2;
    public const byte purpleCircle = 3;
    public const byte greenCircle = 4;
    public const byte blueCircle = 5;

    private float castingTimingLeniency = 0.5f;
    public bool canCast = true;
    private bool castingMagicBolt = false;
    private bool castingFlamethrower = false;

    private float magicBoltCastingTime = 0.5f;

    public GameObject magicBolt;
    public GameObject shield;
    public GameObject flamethrower;
    public GameObject battleField;
    public GameObject spreadShot;
    public GameObject homingBlackHole;

    public NetworkObject currentObject;
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
            else if (castingFlamethrower)
            {
                if (!InputManager.Instance.rightTrigger)
                {
                    DespawnCurrentObjectRPC();
                    castingFlamethrower = false;
                    canCast = true;
                }
            }
        }
    }

    public void CastSpell(InputState input)
    {
        if (canCast)
        {
            if (CheckInput(SpellRequirements.redCircle, input))
            {
                CastMagicCircleRPC(redCircle);
            }
            else if (CheckInput(SpellRequirements.yellowCircle, input))
            {
                CastMagicCircleRPC(yellowCircle);
            }
            else if (CheckInput(SpellRequirements.purpleCircle, input))
            {
                CastMagicCircleRPC(purpleCircle);
            }
            else if (CheckInput(SpellRequirements.greenCircle, input))
            {
                CastMagicCircleRPC(greenCircle);
            }
            else if (CheckInput(SpellRequirements.blueCircle, input))
            {
                CastMagicCircleRPC(blueCircle);
            }
            else if (CheckInput(SpellRequirements.flamethrower, input))
            {
                CastFlamethrowerRPC();
                canCast = false;
                castingFlamethrower = true;
            }
            else if (CheckInput(SpellRequirements.shield, input))
            {
                CastShieldRPC(transform.position);
            }
            else if (CheckInput(SpellRequirements.battlefield, input))
            {
                CastBattleFieldRPC(transform.position);
            }
            else if (CheckInput(SpellRequirements.speedBoost, input))
            {
                NetworkPlayer.local.speedBoostTime = 30f;
            }
            else if (CheckInput(SpellRequirements.SpreadShot, input))
            {
                CastSpreadShotRPC(transform.position, InputManager.Instance.headTransform.up, InputManager.Instance.lookDirection);
            }
            else if (CheckInput(SpellRequirements.homingBlackHole, input))
            {
                CastHomingBlackHoleRPC(transform.position, InputManager.Instance.lookDirection);
            }

            //special cases
            else if (input.yButton || input.bButton)
            {
                CastMagicCircleRPC(0);
                InputTracker.Instance.ResetInputState();
            }
            else if (input.rightZone == 3 && input.rightGrip && input.rightZoneDuration >= magicBoltCastingTime && input.rightGripDuration >= magicBoltCastingTime && input.rightZoneDuration >= input.rightGripDuration)
            {
                InputManager.Instance.VibrateController(true, false);
                InputTracker.Instance.ResetInputState();
                castingMagicBolt = true;
                canCast = false;
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void DespawnCurrentObjectRPC()
    {
        if (currentObject != null)
        {
            currentObject.Despawn();
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
        instance.ownerId.Value = GetComponent<NetworkObject>().NetworkObjectId;
    }

    [Rpc(SendTo.Server)]
    private void CastHomingBlackHoleRPC(Vector3 position, Vector3 direction)
    {
        var instance = Instantiate(homingBlackHole).GetComponent<HomingAOE>();
        instance.direction = direction;
        instance.transform.position = position;
        instance.GetComponent<NetworkObject>().Spawn();
        instance.ownerId.Value = GetComponent<NetworkObject>().NetworkObjectId;
    }

    [Rpc(SendTo.Server)]
    private void CastSpreadShotRPC(Vector3 position, Vector3 startDirection, Vector3 lookDirection) 
    {
        for (int i = 0; i < 8; i++)
        {
            var instance = Instantiate(spreadShot).GetComponent<BaseProjectile>();
            instance.direction = Quaternion.AngleAxis(i * 45, lookDirection) * startDirection;
            instance.transform.Rotate(Vector3.forward, 45 * i);
            instance.transform.position = position;
            instance.GetComponent<NetworkObject>().Spawn();
            instance.ownerId.Value = GetComponent<NetworkObject>().NetworkObjectId;
        }
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
        instance.transform.position = new Vector3(0, -25, 0);
        instance.GetComponent<NetworkObject>().Spawn();
        instance.ownerId.Value = GetComponent<NetworkObject>().NetworkObjectId;
        currentObject = instance.NetworkObject;
    }

    [Rpc(SendTo.Server)]
    private void CastBattleFieldRPC(Vector3 position)
    {
        var instance = Instantiate(battleField).GetComponent<BaseSpell>();
        instance.transform.position = new Vector3(position.x, 0, position.z);
        instance.NetworkObject.Spawn();
        instance.ownerId.Value = NetworkObject.NetworkObjectId;
        if (ServerManager.Instance.battlefield != null)
        {
            ServerManager.Instance.battlefield.NetworkObject.Despawn();
        }
        ServerManager.Instance.battlefield = instance;
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

    private bool CheckInput(Requirements requirements, InputState input)
    {
        if (requirements.innerCircle != 0 && requirements.innerCircle != input.innerCircle)
        {
            return false;
        }
        else
        {
            if (requirements.middleCircle != 0 && requirements.middleCircle != input.middleCircle)
            {
                return false;
            }
            else
            {
                if (requirements.outerCircle != 0 && requirements.outerCircle != input.outerCircle)
                {
                    return false;
                }
                else
                {
                    if (requirements.leftTrigger)
                    {
                        if (input.leftZoneDuration > input.leftTriggerDuration || !input.leftTrigger)
                        {
                            return false;
                        }
                    }
                    if (requirements.leftGrip)
                    {
                        if (input.leftZoneDuration > input.leftGripDuration || !input.leftGrip)
                        {
                            return false;
                        }
                    }

                    if (requirements.rightTrigger)
                    {
                        if (input.rightZoneDuration > input.rightTriggerDuration || !input.rightTrigger)
                        {
                            return false;
                        }
                    }
                    if (requirements.rightGrip)
                    {
                        if (input.rightZoneDuration > input.rightGripDuration || !input.rightGrip)
                        {
                            return false;
                        }
                    }

                    if (requirements.leftZone != 0)
                    {
                        if (input.leftZone != requirements.leftZone || input.prevLeftZone != requirements.prevLeftZone)
                        {
                            return false;
                        }
                    }
                    if (requirements.rightZone != 0)
                    {
                        if (input.rightZone != requirements.rightZone || input.prevRightZone != requirements.prevRightZone)
                        {
                            return false;
                        }
                    }

                    if (requirements.innerCircle != 0)
                    {
                        ConsumeMagicCirclesRPC();
                    }
                    InputTracker.Instance.ResetInputState();
                    return true;
                }
            }
        }
    }
}

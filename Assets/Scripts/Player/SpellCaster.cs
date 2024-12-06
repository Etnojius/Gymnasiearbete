using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpellCaster : NetworkBehaviour
{
    public bool canCast = true;
    private bool castingMagicBolt = false;

    private float magicBoltCastingTime = 0.5f;

    public GameObject magicBolt;

    public InputState receivedInputState;
    public byte rightZone;
    public bool rightGrip;
    public float rightZoneDuration;
    public float rightGripDuration;
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
                    CastMagicBoltRPC(InputManager.Instance.rightHandTransform.position, InputManager.Instance.rightHandMovementDirection);
                    castingMagicBolt = false;
                    canCast = true;
                }
            }
        }
    }

    public void CastSpell(InputState input)
    {
        receivedInputState = input;
        rightZone = input.rightZone;
        rightGrip = input.rightGrip;
        rightZoneDuration = input.rightZoneDuration;
        rightGripDuration = input.rightGripDuration;
        if (canCast)
        {
            if (input.rightZone == 3 && input.rightGrip && input.rightZoneDuration >= magicBoltCastingTime && input.rightGripDuration >= magicBoltCastingTime)
            {
                InputManager.Instance.VibrateController(true, false);
                castingMagicBolt = true;
                canCast = false;
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
    }
}

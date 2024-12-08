using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer local;

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public ParticleSystem innerCirclePSL;
    public ParticleSystem middleCirclePSL;
    public ParticleSystem outerCirclePSL;

    public ParticleSystem innerCirclePSR;
    public ParticleSystem middleCirclePSR;
    public ParticleSystem outerCirclePSR;

    public NetworkVariable<float> hp = new NetworkVariable<float>(100);
    public NetworkVariable<byte> innerCircle = new NetworkVariable<byte>(0);
    public NetworkVariable<byte> middleCircle = new NetworkVariable<byte>(0);
    public NetworkVariable<byte> outerCircle = new NetworkVariable<byte>(0);
    public float maxHP = 100;

    public float speedBoostTime = 0f;
    public float speddBoostMult = 2f;

    public Renderer[] meshToDisable;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            local = this;
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

            VRRigReferences.Instance.hpMeter.fillAmount = hp.Value / maxHP;

            speedBoostTime -= Time.deltaTime;
        }

        if (IsServer)
        {
            if (hp.Value <= 0)
            {
                DeathRPC();
                hp.Value = maxHP;
            }
        }

        if (innerCircle.Value == 0)
        {
            innerCirclePSL.gameObject.SetActive(false);
            innerCirclePSR.gameObject.SetActive(false);
        }
        else
        {
            innerCirclePSL.gameObject.SetActive(true);
            var main = innerCirclePSL.main;
            main.startColor = GetColorFromIndex(innerCircle.Value);
            innerCirclePSR.gameObject.SetActive(true);
            main = innerCirclePSR.main;
            main.startColor = GetColorFromIndex(innerCircle.Value);
        }
        if (middleCircle.Value == 0)
        {
            middleCirclePSL.gameObject.SetActive(false);
            middleCirclePSR.gameObject.SetActive(false);
        }
        else
        {
            middleCirclePSL.gameObject.SetActive(true);
            var main = middleCirclePSL.main;
            main.startColor = GetColorFromIndex(middleCircle.Value);
            middleCirclePSR.gameObject.SetActive(true);
            main = middleCirclePSR.main;
            main.startColor = GetColorFromIndex(middleCircle.Value);
        }
        if (outerCircle.Value == 0)
        {
            outerCirclePSL.gameObject.SetActive(false);
            outerCirclePSR.gameObject.SetActive(false);
        }
        else
        {
            outerCirclePSL.gameObject.SetActive(true);
            var main = outerCirclePSL.main;
            main.startColor = GetColorFromIndex(outerCircle.Value);
            outerCirclePSR.gameObject.SetActive(true);
            main = outerCirclePSR.main;
            main.startColor = GetColorFromIndex(outerCircle.Value);
        }
    }

    private Color GetColorFromIndex(byte index)
    {
        Color color = Color.black;
        switch (index)
        {
            case SpellCaster.redCircle:
                color = Color.red;
                break;
            case SpellCaster.yellowCircle:
                color = Color.yellow;
                break;
            case SpellCaster.purpleCircle:
                color = new Color(148f / 255f, 0, 211f / 255f);
                break;
            case SpellCaster.greenCircle:
                color = Color.green;
                break;
            case SpellCaster.blueCircle:
                color = Color.blue;
                break;
        }
        return color;
    }

    public void TakeDamage(float amount)
    {
        hp.Value -= amount;
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

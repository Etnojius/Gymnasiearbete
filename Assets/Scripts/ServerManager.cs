using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance;
    public bool isActive;
    public BaseSpell battlefield;

    void Start()
    {
        Instance = this;
    }

    public void CancelMagic()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PrivateNetworkManager : MonoBehaviour
{
    private NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartHost()
    {
        networkManager.StartHost();
    }

    public void StartClient()
    {
        networkManager.StartClient();
    }
}

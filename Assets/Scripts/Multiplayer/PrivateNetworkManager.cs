using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using TMPro;
using System;

public class PrivateNetworkManager : MonoBehaviour
{
    private NetworkManager networkManager;

    [SerializeField]
    private TMP_Text joinCodeOutput;
    [SerializeField]
    private TMP_InputField joinCodeInput;
    public GameObject connectionInterface;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void Host()
    {
        InputManager.Instance.VibrateController(true, true);
        connectionInterface.SetActive(false);
        string joinCode = await StartHostWithRelay();
        joinCodeOutput.text = joinCode;
    }

    public async void Join()
    {
        connectionInterface.SetActive(false);
        await StartClientWithRelay(joinCodeInput.text);
    }

    public async Task<string> StartHostWithRelay(int maxConnections = 5)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }

    void HandleClientDisconnected(ulong clientId)
    {
        connectionInterface.SetActive(true);
    }
}

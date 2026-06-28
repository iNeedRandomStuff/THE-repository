using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FishNet.Object.Synchronizing;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Managing;

public class connectionAndNetManager : NetworkBehaviour
{
    [Header("net")]
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private ushort port;

    [Header("players")]
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject player3;
    [SerializeField] private GameObject player4;
    [SerializeField] private GameObject player5;


    void Update()
    {
        showConnectedPlayers();
    }

    public void connect()
    {
        GameManager.IP = ipInput.text;
        string _ip = GameManager.IP;
        if (string.IsNullOrEmpty(_ip))
        {
            print("string is null");
            _ip = "localhost";
        }
        InstanceFinder.ClientManager.StartConnection(GameManager.IP, port);
    }

    [ServerRpc(RequireOwnership = false)]
    void showConnectedPlayers()
    {
        
        print(ServerManager.Clients.Count);
        switch (ServerManager.Clients.Count)
        {
            case 1:
                player1.SetActive(true);
                break;

            case 2:
                player2.SetActive(true);
                break;

            case 3:
                player3.SetActive(true);
                break;

            case 4:
                player4.SetActive(true);
                break;

            case 5:
                player5.SetActive(true);
                break;

            default:
                Debug.Log("No matching object.");
                break;
        }
        
        /*
        foreach (NetworkConnection conn in ServerManager.Clients.Values)
        {
            if (conn.FirstObject == null)
                return;

            CharacterSelector characterSelector = conn.FirstObject.GetComponent<CharacterSelector>();

            if (characterSelector.IsReady.Value == false)
                return;
        }
        */
    }
}

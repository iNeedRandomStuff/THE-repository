using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using TMPro;
using FishNet.Object.Synchronizing;

public class clientCounter : NetworkBehaviour
{
    public GameObject thisObject;

    [SerializeField] private int players;
    [SerializeField] private TMP_Text TMPtext;

    [SerializeField] public readonly SyncVar<float> clientAmount = new SyncVar<float>();


    void Update()
    {
        counter();
    }

    void counter()
    {
        clientAmount.Value = InstanceFinder.ServerManager.Clients.Count;
        TMPtext.text = "" + clientAmount.Value;
    }
}

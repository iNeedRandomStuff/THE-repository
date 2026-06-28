using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using TMPro;

public class clientCounter : NetworkBehaviour
{
    [SerializeField] private int players;
    [SerializeField] private TMP_Text TMPtext;

    void Update()
    {
        counter();
    }

    void counter()
    {
        players = InstanceFinder.ServerManager.Clients.Count;
        TMPtext.text = "" + players;
    }
}

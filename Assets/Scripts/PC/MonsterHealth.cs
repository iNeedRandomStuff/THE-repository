using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public class MonsterHealth : NetworkBehaviour
{
    public readonly SyncVar<int> health = new SyncVar<int>();

    [SerializeField] private int damage;
    [SerializeField] private int startHealth;
    [SerializeField] private GameObject Self;

    public GameObject cosmonautsWonSign;
    public GameObject monsterWonSign;

    void Start()
    {
        monsterWonSign = GameObject.FindGameObjectWithTag("TheMonsterWon");
        cosmonautsWonSign = GameObject.FindGameObjectWithTag("CosmonautWonSign");
        if (IsServerInitialized)
            health.Value = startHealth;
    }

    public void Health()
    {
        Debug.Log("Health activated");
        ServerHealth(damage, Self);
        Debug.Log(health.Value + "network health");
    }

    [ServerRpc(RequireOwnership = false)]
    public void ServerHealth(int _amountToChange, GameObject _obj)
    {
        health.Value -= _amountToChange;
        print("value changed");

        if (health.Value <= 0)
        {
            ObserverHealth(_obj);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ServerHealOnKill()
    {
        health.Value = startHealth;
    }

    [ObserversRpc]
    public void ObserverHealth(GameObject _obj)
    {
        print("cosmonauts won!");
        cosmonautsWonSign.transform.localScale = cosmonautsWonSign.transform.localScale * 1000;
        ServerManager.Despawn(_obj);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
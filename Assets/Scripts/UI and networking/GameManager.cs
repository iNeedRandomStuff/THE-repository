using FishNet.Object.Synchronizing;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public static string IP;
    public static float sensetivity;
    public static float playerVoice;
    public static float SFX;

    public static List<mainMenuUIinteractor> m_Players = new List<mainMenuUIinteractor>();

    [Header("prefabs")]
    public GameObject vr;
    public GameObject pc;
    public GameObject PcAstronautTestAgent;

    [Header("spawn points")]
    [SerializeField] private Transform vrSpawn;
    [SerializeField] private Transform pcSpawn;

    [Header ("settings")]
    [SerializeField] private bool spawnPlayers;

    void Start()
    {
        if (!InstanceFinder.IsServerStarted)
            return;

        if (spawnPlayers == true)
        {
            StartGameServer();
        }
    }

    public static void RegisterPlayer(mainMenuUIinteractor player)
    {
        m_Players.Add(player);
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartGameServer()
    {
        foreach (NetworkConnection conn in ServerManager.Clients.Values)
        {
            if (conn.FirstObject == null)
                return;

            CharacterSelector characterSelector = conn.FirstObject.GetComponent<CharacterSelector>();

            if (characterSelector.IsReady.Value == false)
                return;
        }

        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        foreach (NetworkConnection conn in ServerManager.Clients.Values)
        {
            CharacterSelector characterSelector = conn.FirstObject.GetComponent<CharacterSelector>();

            //GameObject _prefab = characterSelector.SelectedCharacter.Value == 1 ? vr : pc;
            GameObject _prefab = null;

            if (characterSelector.SelectedCharacter.Value == 1)
            {
                _prefab = vr;
            }
            else
            {
                if (characterSelector.SelectedCharacter.Value == 2)
                {
                    _prefab = pc;
                }
                else
                {
                    if (characterSelector.SelectedCharacter.Value == 3)
                    {
                        _prefab = PcAstronautTestAgent;
                    }
                }
            }
            //Transform _spawnPoint = characterSelector.SelectedCharacter.Value == 1 ? vrSpawn : pcSpawn;
            Transform _spawnPoint = null;

            if (characterSelector.SelectedCharacter.Value == 2)
            {
                _spawnPoint = pcSpawn;
            }
            else
            {
                _spawnPoint = vrSpawn;
            }
            conn.FirstObject.Despawn();

            GameObject _player = Instantiate(_prefab, _spawnPoint.position, _spawnPoint.rotation);
            ServerManager.Spawn(_player, conn);
        }
    }
}

using FishNet.Object;
using FishNet.Connection;
using UnityEngine;

public class customPlayerSpawner : NetworkBehaviour
{
    [Header("prefabs")]
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject astronaut;

    [Header("spawnPoints")]
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private Transform astronautSpawnPoint;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        foreach (NetworkConnection conn in ServerManager.Clients.Values)
        {
            if (conn.FirstObject == null)
                return;

            emptyPlayerValues emptyPlayerValuesScript = conn.FirstObject.GetComponent<emptyPlayerValues>();

            GameObject _prefab = null;

            if(emptyPlayerValuesScript.SelectedCharacter.Value == 1)
            {
                _prefab = monster;
            }
            else
            {
                if (emptyPlayerValuesScript.SelectedCharacter.Value == 2)
                {
                    _prefab = astronaut;
                }
            }

            Transform _spawnPoint = null;

            if (emptyPlayerValuesScript.SelectedCharacter.Value == 1)
            {
                _spawnPoint = monsterSpawnPoint;
            }
            else
            {
                _spawnPoint = astronautSpawnPoint;
            }
            //conn.FirstObject.Despawn();

            GameObject _player = Instantiate(_prefab, _spawnPoint.position, _spawnPoint.rotation);
            ServerManager.Spawn(_player, conn);
        }
    }

    /*
    private void SpawnPlayers()
    {
        foreach (NetworkConnection conn in ServerManager.Clients.Values)
        {
            CharacterSelector characterSelector = conn.FirstObject.GetComponent<CharacterSelector>();

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
    */
}

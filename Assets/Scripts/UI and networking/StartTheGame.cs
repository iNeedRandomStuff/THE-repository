using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine;

public class StartTheGame : NetworkBehaviour
{
    /*
    [Header ("prefabs")]
    public GameObject vr;
    public GameObject pc;
    public GameObject PcAstronautTestAgent;

    [Header("spawn points")]
    [SerializeField] private Transform vrSpawn;
    [SerializeField] private Transform pcSpawn;

    [Header ("UI")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject menuButtons;
    [SerializeField] private GameObject cosmonautsWon;
    [SerializeField] private GameObject monsterWon;

    public void StartGameButton()
    {
        if (!InstanceFinder.IsServerStarted)
            return;

        StartGameServer();
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

        HideMainMenuObservers();
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
                if(characterSelector.SelectedCharacter.Value == 2)
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

    [ObserversRpc]
    private void HideMainMenuObservers()
    {
        menuButtons.SetActive(true);
        mainMenu.SetActive(false);
    }
    */

    [SerializeField] private GameManager gameManager;
    [SerializeField] private CharacterSelector CharacterSelector;

    void Awake()
    {
        if (!InstanceFinder.IsServerStarted)
        {
            gameObject.SetActive(false);
        }
    }

    public void switchScene()
    {
        foreach (NetworkConnection conn in ServerManager.Clients.Values)
        {
            if (conn.FirstObject == null)
                return;

            CharacterSelector characterSelector = conn.FirstObject.GetComponent<CharacterSelector>();

            if (characterSelector.IsReady.Value == false)
                return;
        }

        gameManager.spawnPlayers = true;
        SceneLoadData sld = new SceneLoadData("gameplay_scene");
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
        UnladScene("main_menu_room");
    }

    void UnladScene(string sceneName)
    {
        SceneUnloadData sld = new SceneUnloadData(sceneName);
        InstanceFinder.SceneManager.UnloadGlobalScenes(sld);
    }
}
using FishNet;
using FishNet.Managing;
using UnityEngine;

public class HostLobby : MonoBehaviour
{
    [SerializeField] private ushort port = 7770;
    [SerializeField] private GameObject menuButtons;

    public void HostLobbyFunction()
    {
        InstanceFinder.ServerManager.StartConnection(port);
        InstanceFinder.ClientManager.StartConnection("localhost", port);

        menuButtons.SetActive(false);
    }
}

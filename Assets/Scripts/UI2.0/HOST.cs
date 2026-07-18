using FishNet;
using UnityEngine;
using FishNet.Transporting;
using FishNet.Managing.Scened;

public class HOST : MonoBehaviour
{
    public void Host()
    {
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();;
    }

    private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            InstanceFinder.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;

            SceneLoadData sld = new SceneLoadData("main_menu_room");
            InstanceFinder.SceneManager.LoadGlobalScenes(sld);
        }
    }
}

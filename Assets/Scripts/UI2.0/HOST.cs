using FishNet;
using UnityEngine;

public class HOST : MonoBehaviour
{
    public void Host()
    {
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();
    }
}

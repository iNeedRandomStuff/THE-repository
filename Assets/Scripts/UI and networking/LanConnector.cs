using TMPro;
using FishNet;
using FishNet.Managing;
using UnityEngine;

public class LanConnector : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private ushort port;
    [SerializeField] private GameObject mainMenu;

    public void StartClient()
    {
        string _ip = ipInput.text;
        if (string.IsNullOrEmpty(_ip))
        {
            print("string is null");
            _ip = "localhost";
        }

        InstanceFinder.ClientManager.StartConnection(_ip, port);
        mainMenu.SetActive(false);
    }
}

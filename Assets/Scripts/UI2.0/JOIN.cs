using TMPro;
using FishNet;
using FishNet.Managing;
using UnityEngine;

public class JOIN : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private ushort port;
    //[SerializeField] private GameObject mainMenu;

    [Header("net")]
    [SerializeField] private NetworkManager networkManager;

    public void StartClient()
    {
        if (!networkManager.ClientManager.Started)
        {
            string _ip = ipInput.text;

            if (string.IsNullOrEmpty(_ip))
            {
                print("string is null");
            }
            else
            {
                InstanceFinder.ClientManager.StartConnection(_ip, port);
            }
        }
    }
}

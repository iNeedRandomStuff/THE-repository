using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine;

public class startthegame : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
        {
            gameObject.SetActive(false);
        }
    }
    /*
    public override void OnStartServer()
    {
        base.OnStartServer();
        SceneLoadData sld = new SceneLoadData("main_menu_room");
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }
    */
    public void StartTheGame()
    {
        foreach (NetworkConnection conn in ServerManager.Clients.Values)
        {
            if (conn.FirstObject == null)
                return;

            emptyPlayerValues emptyPlayerValuesScript = conn.FirstObject.GetComponent<emptyPlayerValues>();

            if (emptyPlayerValuesScript.IsReady.Value == false)
                return;
        }

        switchScene();
    }

    void switchScene()
    {
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

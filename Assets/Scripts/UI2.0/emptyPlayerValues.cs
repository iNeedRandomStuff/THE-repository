using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class emptyPlayerValues : NetworkBehaviour
{
    private GameObject choiceObject;
    private GameObject clientCounterObject;

    private chooseRole chooseRoleScript;
    private clientCounter clientCounterScript;

    [SerializeField] public readonly SyncVar<bool> IsReady = new SyncVar<bool>();
    [SerializeField] public readonly SyncVar<int> SelectedCharacter = new SyncVar<int>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            chooseRoleScript = Object.FindAnyObjectByType<chooseRole>(FindObjectsInactive.Include);
            clientCounterScript = Object.FindAnyObjectByType<clientCounter>(FindObjectsInactive.Include);

            chooseRoleScript.emptyPlayerValuesScript = gameObject.GetComponent<emptyPlayerValues>();

            choiceObject = chooseRoleScript.thisObject;
            clientCounterObject = clientCounterScript.thisObject;
            //initializeUI(choiceObject, clientCounterObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void initializeUI(GameObject _chooseRoleScriptObj, GameObject _clientCounterObj)
    {
        ServerManager.Spawn(_chooseRoleScriptObj);
        ServerManager.Spawn(_clientCounterObj);
    }
}

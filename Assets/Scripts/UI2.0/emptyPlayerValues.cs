using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class emptyPlayerValues : NetworkBehaviour
{
    [SerializeField] private GameObject choiceObject;
    private chooseRole chooseRoleScript;

    [SerializeField] public readonly SyncVar<bool> IsReady = new SyncVar<bool>();
    [SerializeField] public readonly SyncVar<int> SelectedCharacter = new SyncVar<int>();

    void Start()
    {
        chooseRoleScript = Object.FindAnyObjectByType<chooseRole>();
        chooseRoleScript.emptyPlayerValuesScript = gameObject.GetComponent<emptyPlayerValues>();
    }


}

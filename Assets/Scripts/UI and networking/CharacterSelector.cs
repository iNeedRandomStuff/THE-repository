using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : NetworkBehaviour
{
    [Header("UI")]
    public GameObject VrPcSelectorPanel;
    public Canvas CanvasObj;

    public readonly SyncVar<int> SelectedCharacter = new SyncVar<int>();
    public readonly SyncVar<bool> IsReady = new SyncVar<bool>();

    //public GameObject spawn;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if(base.IsOwner)
        {
            IsReady.Value = false;
            CanvasObj.gameObject.SetActive(true);
        }
    }

    // UI
    public void SelectVR()
    {
        if (!IsOwner)
            return;

        VrPcSelectorPanel.SetActive(false);
        SetCharacterServerRpc(1);
    }

    public void SelectPC()
    {
        if (!IsOwner)
            return;

        VrPcSelectorPanel.SetActive(false);
        SetCharacterServerRpc(2);
    }

    public void SelectPcAstronautTestAgent()
    {
        if (!IsOwner)
            return;

        VrPcSelectorPanel.SetActive(false);
        SetCharacterServerRpc(3);
    }

    // not UI
    [ServerRpc]
    private void SetCharacterServerRpc(int _characterId)
    {
        SelectedCharacter.Value = _characterId;
        IsReady.Value = true;
    }
}

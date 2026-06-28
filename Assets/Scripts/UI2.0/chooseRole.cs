using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class chooseRole : NetworkBehaviour
{
    public readonly SyncVar<bool> IsReady = new SyncVar<bool>();
    public readonly SyncVar<int> SelectedCharacter = new SyncVar<int>();


    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            IsReady.Value = false;
        } 
    }

    public void chooseMonster()
    {
        choice(1);
    }

    public void chooseCosmonaut()
    {
        choice(2);
    }

    void choice(int _characterId)
    {
        SelectedCharacter.Value = _characterId;
        IsReady.Value = true;
    }
}

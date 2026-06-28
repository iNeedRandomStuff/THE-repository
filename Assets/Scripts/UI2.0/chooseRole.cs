using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class chooseRole : NetworkBehaviour
{
    public emptyPlayerValues emptyPlayerValuesScript;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            //IsReady.Value = false;
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

    public void choice(int _characterId)
    {
        emptyPlayerValuesScript.SelectedCharacter.Value = _characterId;
        emptyPlayerValuesScript.IsReady.Value = true;
        print(emptyPlayerValuesScript.SelectedCharacter.Value);
    }
}
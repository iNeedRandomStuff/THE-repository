using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;


public class ElevatorButton : NetworkBehaviour
{
    private bool allSet;

    [SerializeField] private AllLeveresPushed allLeversPushedScript;
    [SerializeField] private Animator animator;

    void Start()
    {
        InvokeRepeating(nameof(SetElevator), 0f, 1.0f);
    }

    void SetElevator()
    {
        if (allLeversPushedScript.elevatorSet.Value == true)
        {
            print("all set");
            allSet = true;
        }
    }

    public void Function()
    {
        print("function activated");
        if(allSet)
        {
            PressButtonServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PressButtonServerRpc()
    {
        PressButtonObserversRpc();
    }

    [ObserversRpc]
    private void PressButtonObserversRpc()
    {
        animator.SetTrigger("ElevatorButtonPsuhed");
    }
}

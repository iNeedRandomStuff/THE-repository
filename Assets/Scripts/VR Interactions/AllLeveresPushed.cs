using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public class AllLeveresPushed : NetworkBehaviour
{
    public List<Levers> ListOfLevers;

    public readonly SyncVar<int> currentLeverGroupsPulled = new SyncVar<int>();
    public readonly SyncVar<bool> elevatorSet = new SyncVar<bool>();

    [Header("Threshholds")]
    [SerializeField] private int elevatorThreshhold;
    [SerializeField] private int winThreshhold;

    [Header("Win signs")]
    public GameObject cosmonautsWonSign;


    void Start()
    {
        if(base.IsServerInitialized)
        {
            cosmonautsWonSign = GameObject.FindGameObjectWithTag("CosmonautWonSign");
            InvokeRepeating(nameof(CheckLevers), 0f, 1.0f);
        }
    }

    void CheckLevers()
    {
        for(int i = 0; i < ListOfLevers.Count; i++)
        {
            if (ListOfLevers[i] != null)
            {
                if (ListOfLevers[i].BothleversPulled.Value == true)
                {
                    currentLeverGroupsPulled.Value += 1;
                    ListOfLevers.RemoveAt(i);
                    if (currentLeverGroupsPulled.Value >= elevatorThreshhold)
                    {
                        elevatorSet.Value = true;
                        if (currentLeverGroupsPulled.Value >= winThreshhold)
                        {
                            print("cosmonauts won");
                            cosmonautsWonSign.transform.localScale = cosmonautsWonSign.transform.localScale * 1000;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Lever element" + i + "has not been assigned");
            }
        }
    }
}

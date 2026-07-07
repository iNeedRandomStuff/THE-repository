using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hologramProjector : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    public void Function()
    {
        if(UI.active == false)
        {
            UI.SetActive(true);
        }
        else
        {
            UI.SetActive(false);
        }
    }
}
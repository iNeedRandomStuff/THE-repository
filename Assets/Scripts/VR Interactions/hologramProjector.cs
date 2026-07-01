using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hologramProjector : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    public void Function()
    {
        UI.SetActive(true);
    }
}
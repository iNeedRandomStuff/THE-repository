using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hologramProjector : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    public void Function()
    {
        UI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void turnOffUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UI.SetActive(false);
    }
}
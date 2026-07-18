using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hologramProjector : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    [Header("positions")]
    [SerializeField] private Transform readyToCliclPos;
    [SerializeField] private Transform BackButton;
    [SerializeField] private Transform Settings;
    [SerializeField] private Transform Disconnect;
    [SerializeField] private Vector3 offset;

    [Header("controller")]
    [SerializeField] private Transform leftController;
    [SerializeField] private float speed;

    bool continueMovementToReadyPos;
    bool continueMovementToButton;
    Transform buttonInQuestion;

    void Update()
    {
        float step = speed * Time.deltaTime;
        if (continueMovementToReadyPos == true)
        {
            leftController.position = Vector3.MoveTowards(leftController.position, readyToCliclPos.position, step);
            leftController.rotation = readyToCliclPos.rotation;
            if (leftController.position == readyToCliclPos.position)
            {
                continueMovementToReadyPos = false;
            }
        }

        if (continueMovementToButton == true)
        {
            leftController.position = Vector3.MoveTowards(leftController.position, buttonInQuestion.position, step);
            if(leftController.position == buttonInQuestion.position + offset)
            {
                continueMovementToButton = false;
                continueMovementToReadyPos = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            clickBack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ClickDisconnect();
        }
    }

    public void Function()
    {
        UI.SetActive(true);
        continueMovementToReadyPos = true;
    }

    public void turnOffUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UI.SetActive(false);
    }

    void clickBack()
    {
        continueMovementToButton = true;
        buttonInQuestion = BackButton;
    }
    
    void ClickDisconnect()
    {
        continueMovementToButton = true;
        buttonInQuestion = Disconnect;
    }
}
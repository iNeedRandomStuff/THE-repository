using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PcVrCompatability : NetworkBehaviour
{
    [Header("Pistol")]
    [SerializeField] private Transform rightController;
    [SerializeField] private Transform pistolHolster;
    [SerializeField] private Transform defaultPistolPos;
    [SerializeField] private GrabInteractable grabInteractableScriptRight;

    [Header("Flashlight")]
    [SerializeField] private Transform leftController;
    [SerializeField] private Transform flashlightHolster;
    [SerializeField] private Transform defaultFlashlightlPos;
    [SerializeField] private GrabInteractable grabInteractableScriptLeft;

    private bool alfa1Pressed;
    private bool alfa2Pressed;
    private Vector3 hitPoint;

    private bool initializedPC;

    [Header("camera Initialization")]
    [SerializeField] private float speed;
    [SerializeField] private Transform camera;
    [SerializeField] private float cameraOffset;
    [SerializeField] private Transform camPos;

    [Header("lever activation")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask ignoreLayer;
    private Levers levers;


    void Update()
    {
        initializePc();

        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, layer & ~ignoreLayer))
        {
            hitPoint = hit.point;
            if (Input.GetKeyDown(KeyCode.F) && hit.distance <= range)
            {
                if (hit.transform.tag == "Interactable")
                {
                    if (hit.transform.TryGetComponent<Levers>(out Levers _levers))
                    {
                        levers = _levers;
                        levers.Function();
                        levers = null;
                    }
                }
            }
        }
        float _step = speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            alfa1Pressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            alfa2Pressed = true;
        }

        if (alfa1Pressed == true)
        {
            pullPistol(_step);
        }

        if(alfa2Pressed == true)
        {
            pullFlashlight(_step);
        }

        grabInteractableScriptRight.pcUse = false;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            grabInteractableScriptRight.pcUse = true;
        }

        grabInteractableScriptLeft.pcUse = false;
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            grabInteractableScriptLeft.pcUse = true;
        }
    }

    void pullPistol(float _step)
    {
        if (grabInteractableScriptRight.InteractableObject != null)
        {
            rightController.position = Vector3.MoveTowards(rightController.position, defaultPistolPos.position, _step);
            rightController.LookAt(hitPoint);
        }
        else
        {
            grabInteractableScriptRight.autoGrab = true;
            rightController.position = Vector3.MoveTowards(rightController.position, pistolHolster.position, _step);
        }
    }

    void pullFlashlight(float _step)
    {
        if (grabInteractableScriptLeft.InteractableObject != null)
        {
            leftController.position = Vector3.MoveTowards(leftController.position, defaultFlashlightlPos.position, _step);
            leftController.LookAt(hitPoint);
        }
        else
        {
            grabInteractableScriptLeft.autoGrab = true;
            leftController.position = Vector3.MoveTowards(leftController.position, flashlightHolster.position, _step);
        }
    }

    void initializePc()
    {
        if(initializedPC == false)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                camera.position = camPos.position;
                camera.position += camera.up * cameraOffset;
                initializedPC = true;
            }
        }
    }
}

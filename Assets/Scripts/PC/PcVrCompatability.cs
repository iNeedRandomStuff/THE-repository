using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PcVrCompatability : NetworkBehaviour
{
    [Header("right hand")]
    [SerializeField] private Transform rightController;
    [SerializeField] private Transform defaultPistolPos;
    [SerializeField] private GrabInteractable grabInteractableScriptRight;

    [Header("left hand")]
    [SerializeField] private Transform leftController;
    [SerializeField] private Transform defaultFlashlightlPos;
    [SerializeField] private GrabInteractable grabInteractableScriptLeft;

    [Header("interactable objects")]
    [SerializeField] private Transform pistolHolster;
    [SerializeField] private Transform hologramProjectorHolster;
    [SerializeField] private Transform flashlightHolster;

    [Header("camera Initialization")]
    [SerializeField] private float speed;
    [SerializeField] private Transform camera;
    [SerializeField] private float cameraOffset;
    [SerializeField] private Transform camPos;

    [Header("lever activation")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask ignoreLayer;

    //bools n what not
    private bool hasObjectInHandR;
    private bool hasObjectInHandL;

    private bool continueMovementPistol;
    private bool continueMovementHologramProjector;
    private bool continueMovementFlashlight;
    private bool initializedPC;

    private Vector3 hitPoint;
    [HideInInspector] public Vector3 sway;

    private Levers levers;

    float frameWait;


    void Update()
    {
        initializePc();

        //raycast
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

        // pistol block
        if (Input.GetKeyDown(KeyCode.Alpha1) || continueMovementPistol == true)
        {
            pullPistol(_step);
        }

        if(hasObjectInHandR == true)
        {
            rightController.LookAt(hitPoint);
        }

        //hologram projector block
        if (Input.GetKeyDown(KeyCode.Alpha3) || continueMovementHologramProjector == true)
        {
            pullHlogramProjcetor(_step);
        }

        // flashlight block
        if (Input.GetKeyDown(KeyCode.Alpha2) || continueMovementFlashlight == true)
        {
            pullFlashlight(_step);
        }

        if (hasObjectInHandL == true)
        {
            leftController.LookAt(hitPoint);
        }

        //usage block
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

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            grabInteractableScriptRight.autoGrab = false;
        }
    }

    void pullPistol(float _step)
    {
        // drop object in hand > move hand to pistol holster > pick up pistol > move hand to aim position

        grabInteractableScriptRight.autoGrab = false;
        continueMovementPistol = true;
        hasObjectInHandR = false;
        if(frameWait > 0f)
        {
            if (grabInteractableScriptRight.InteractableObject == null)
            {
                rightController.position = Vector3.MoveTowards(rightController.position, pistolHolster.position, _step);
            }
            else
            {
                grabInteractableScriptRight.autoGrab = true;
                rightController.position = Vector3.MoveTowards(rightController.position, defaultPistolPos.position, _step);
                rightController.rotation = defaultPistolPos.rotation;
                if (rightController.position == defaultPistolPos.position)
                {
                    continueMovementPistol = false;
                    hasObjectInHandR = true;
                    frameWait = 0f;
                }
            }
        }
        else
        {
            frameWait = 1f;
        }
    }

    void pullHlogramProjcetor(float _step)
    {
        // drop object in hand > move hand to pistol holster > pick up pistol > move hand to aim position

        grabInteractableScriptRight.autoGrab = false;
        continueMovementHologramProjector = true;
        hasObjectInHandR = false;
        if(frameWait > 0f)
        {
            if (grabInteractableScriptRight.InteractableObject == null)
            {
                rightController.position = Vector3.MoveTowards(rightController.position, hologramProjectorHolster.position, _step);
            }
            else
            {
                grabInteractableScriptRight.autoGrab = true;
                rightController.position = Vector3.MoveTowards(rightController.position, defaultPistolPos.position, _step);
                rightController.rotation = defaultPistolPos.rotation;
                if (rightController.position == defaultPistolPos.position)
                {
                    continueMovementHologramProjector = false;
                    hasObjectInHandR = true;
                    frameWait = 0f;
                }
            }
        }
        else
        {
            frameWait = 1f;
        }

    }

    void pullFlashlight(float _step)
    {
        // drop object in hand > move hand to falshlight holster > pick up falshlight > move hand to aim position

        grabInteractableScriptLeft.autoGrab = false;
        continueMovementFlashlight = true;
        hasObjectInHandL = false;
        if (grabInteractableScriptLeft.InteractableObject == null)
        {
            leftController.position = Vector3.MoveTowards(leftController.position, flashlightHolster.position, _step);
        }
        else
        {
            grabInteractableScriptLeft.autoGrab = true;
            leftController.position = Vector3.MoveTowards(leftController.position, defaultFlashlightlPos.position, _step);
            leftController.rotation = defaultFlashlightlPos.rotation;
            if (leftController.position == defaultFlashlightlPos.position)
            {
                continueMovementFlashlight = false;
                hasObjectInHandL = true;
            }
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

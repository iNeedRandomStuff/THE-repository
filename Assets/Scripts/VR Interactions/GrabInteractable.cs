using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

public class GrabInteractable : NetworkBehaviour
{
    //attach points
    public Transform AttachPoint;
    private Transform Holster;

    [Header("Inputs")]
    public InputActionProperty Grip;
    public InputActionProperty Trigger;

    //bools
    private bool canGrab;
    private bool interactableIsOurs;
    private bool hasObjectInHand;
    private bool interactableStationary;
    public bool autoGrab;
    public bool pcUse;

    // scripts
    private Interactable interactableScript;
    private Pistol pistol;
    private Flashlight flashlight;
    private Levers levers;
    private ElevatorButton elevatorButton;

    // game objects
    public GameObject InteractableObject;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            gameObject.GetComponent<GrabInteractable>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Interactable" && hasObjectInHand == false)
        {
            interactableScript = collider.gameObject.GetComponent<Interactable>();

            if (interactableScript.ownerName != gameObject.transform.name)
            {
                Holster = interactableScript.Holster;
                InteractableObject = collider.gameObject;
                canGrab = true;
                if(interactableScript.stationary == true)
                {
                    interactableStationary = true;
                }
                if (collider.TryGetComponent<Pistol>(out Pistol _pistol))
                {
                    pistol = _pistol;
                }
                else
                {
                    if (collider.TryGetComponent<Flashlight>(out Flashlight _flashlight))
                    {
                        flashlight = _flashlight;
                    }
                    else
                    {
                        if (collider.TryGetComponent<Levers>(out Levers _levers))
                        {
                            levers = _levers;
                            InteractableObject = null;
                        }
                        else
                        {
                            if (collider.TryGetComponent<ElevatorButton>(out ElevatorButton _elevatorsButton))
                            {
                                elevatorButton = _elevatorsButton;
                                InteractableObject = null;
                            }
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (canGrab == true)
        {
            Interract();
        }
    }

    public void Interract()
    {
        float gripValue = Grip.action.ReadValue<float>();
        var triggerAction = Trigger.action;
        print(InteractableObject);
        if (gripValue > 0.1f || autoGrab == true)
        {
            if(InteractableObject != null)
            {
                InteractableObject.transform.position = AttachPoint.transform.position;
                InteractableObject.transform.rotation = AttachPoint.transform.rotation;
                interactableScript.ownerName = gameObject.transform.name;
                hasObjectInHand = true;
                if(pistol != null)
                {
                    pistol.Hand = gameObject;
                }
            }
            
            if(interactableStationary == true)
            {
                interactableScript.ownerName = gameObject.transform.name;
                hasObjectInHand = true;
            }
        }
        else
        {
            if(interactableStationary == true)
            {
                levers = null;
                elevatorButton = null;
                interactableScript.ownerName = null;
                Holster = null;
                hasObjectInHand = false;
                canGrab = false;
                interactableStationary = false;
            }

            if (InteractableObject != null)
            {
                InteractableObject.transform.position = Holster.transform.position;
                InteractableObject.transform.rotation = Holster.transform.rotation;
                interactableScript.ownerName = null;
                Holster = null;
                InteractableObject = null;
                canGrab = false;
                pistol = null;
                flashlight = null;
                hasObjectInHand = false;
            }
        }

        if (triggerAction.WasPressedThisFrame() || pcUse == true)
        {
            if (pistol != null)
                pistol.Function();
            else if (flashlight != null)
                flashlight.Function();
            else if (levers != null)
                levers.Function();
            else if (elevatorButton != null)
                elevatorButton.Function();
        }
    }
}
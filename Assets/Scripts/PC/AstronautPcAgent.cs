using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class AstronautPcAgent : NetworkBehaviour
{
    private Levers levers;
    [SerializeField] private Transform pistolTransfrom;
    [SerializeField] private Transform flashlightTransfrom;
    [SerializeField] private Transform cam;
    [SerializeField] private float range = 2f;
    [SerializeField] private LayerMask layer;


    private Pistol pistol;
    private Flashlight flashlight;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            gameObject.GetComponent<AstronautPcAgent>().enabled = false;
        }
        else
        {
            pistol = pistolTransfrom.GetComponent<Pistol>();
            flashlight = flashlightTransfrom.GetComponent<Flashlight>();
        }
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, layer.value))
        {
            pistolTransfrom.LookAt(hit.point);
            flashlightTransfrom.LookAt(hit.point);

            if (Input.GetKeyDown(KeyCode.F) && hit.distance <= range)
            {
                if (hit.transform.tag == "Interactable")
                {
                    if(hit.transform.TryGetComponent<Levers>(out Levers _levers))
                    {
                        levers = _levers;
                        levers.Function();
                        levers = null;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            pistol.Function();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            flashlight.Function();
        }
    }
}

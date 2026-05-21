using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFeetPC : MonoBehaviour
{
    [Header("base setup")]
    [SerializeField] private float halfStepLength;
    [SerializeField] private float sideOffset;
    [SerializeField] private float speed;
    [SerializeField] private float stepHeight;
    [SerializeField] private Transform body;
    [SerializeField] private LayerMask terrainLayer;

    [Header ("other foot")]
    [SerializeField] private IKFeetPC otherFoot;
    public bool HasSteppedLast;

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip stepclip;

    private bool idle;
    private Vector3 newPosition;
    private Vector3 oldPosition;


    [SerializeField] private IKFeetVR ikfeetVR;
    private bool vrIKdisabled;

    private float oldPosHitPosDiff;

    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            moveFeetForward();
            if(vrIKdisabled == false)
            {
                ikfeetVR.enabled = false;
                vrIKdisabled = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            moveFeetRight();
            if (vrIKdisabled == false)
            {
                ikfeetVR.enabled = false;
                vrIKdisabled = true;
            }
        }

        if(!Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.S))
        {
            print("no input");
            if (idle == false)
            {
                IdleStep();
            }
        }
    }

    // doesnt work unless player presses key more than once + doesnt actually move target
    void moveFeetForward()
    {
        Ray ray = new Ray(body.position + (body.forward * (idle ? halfStepLength : halfStepLength * 2) + body.right * sideOffset), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5f, terrainLayer.value))
        {
            oldPosHitPosDiff = Vector3.Distance(oldPosition, hit.point);
            if (oldPosHitPosDiff >= halfStepLength * 2)
            {
                newPosition = hit.point;
                executeMovement();
                oldPosition = transform.position;
                idle = false;
            }
        }
    }

    void moveFeetRight()
    {
        idle = false;
    }

    //moves foot every frame, need fix
    void IdleStep()
    {
        Ray ray = new Ray(body.position + (body.right * sideOffset), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5f, terrainLayer.value))
        {
            newPosition = hit.point;
            executeMovement();
            oldPosition = transform.position;
        }
        idle = true;
    }

    void executeMovement()
    {
        transform.position = Vector3.Slerp(oldPosition, newPosition, speed);
        HasSteppedLast = true;
        otherFoot.HasSteppedLast = false;
        if (stepclip != null)
        {
            audioSource.PlayOneShot(stepclip);
        }
    }
}

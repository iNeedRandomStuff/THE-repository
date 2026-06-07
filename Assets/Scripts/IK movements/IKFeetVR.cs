using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFeetVR : MonoBehaviour
{
    //I labeld what I could, but this script still confuses and frightens me. Touch as little as possible for now. its also probably full of useless checks and stuff, coz of how many itereations it wnet through.
    //probably should clean up it later and move it all into different functions

    [Header("main IK")]
    //[SerializeField] private float footSpacing;
    [SerializeField] private float stepDistance;
    [SerializeField] private float stepHeight;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private Transform body;
    [SerializeField] private LayerMask terrainLayer;

    private float lerp;
    private float speed;
    private Vector3 newPosition;
    private Vector3 oldPosition;
    private Vector3 currentPosition;

    [Header("idle stepping")]
    [SerializeField] private Transform restPos;
    private bool idle = false;
    private bool inIdlePosition;

    [Header("alternating feet")]
    [SerializeField] private IKFeetVR otherFoot;
    public bool HasSteppedLast;

    [Header("offsets")]
    [SerializeField] private Vector3 footRotationOffset;
    [SerializeField] private float footYOffset;
    [SerializeField] private float sideOffset;

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip stepclip;

    // velocity & direction
    private Vector3 velPreviousPos;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private float currentSpeedMagnitude;

    void LateUpdate()
    {
        checkVelocity();
        StepOrIdleStep();
    }

    private void checkVelocity()
    {
        velocity = (body.position - velPreviousPos) / Time.deltaTime;
        velPreviousPos = body.position;

        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        currentSpeedMagnitude = horizontalVelocity.magnitude;

        if (currentSpeedMagnitude > 0.1f)
        {
            moveDirection = horizontalVelocity.normalized;
            idle = false;
        }
        else
        {
            idle = true;
        }

        speed = currentSpeedMagnitude + 2f * speedMultiplier;
        transform.position = currentPosition + Vector3.up * footYOffset;
    }
    
    private void StepOrIdleStep()
    {
        Vector3 footDirectionOffset = idle ? Vector3.zero : (moveDirection * stepDistance);

        
        Vector3 rayOrigin = body.position + footDirectionOffset + (body.right * sideOffset);
        Ray ray = new Ray(rayOrigin, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, 5f, terrainLayer.value))
        {
            if(lerp >= 1f && HasSteppedLast == false)
            {
                if (Vector3.Distance(newPosition, info.point) > stepDistance)
                {
                    if (!idle)
                    {
                        lerp = 0;
                        newPosition = info.point;
                        inIdlePosition = false;
                    }
                }
                if (idle && !inIdlePosition)
                {
                    lerp = 0;
                    oldPosition = currentPosition;
                    newPosition = restPos.position + new Vector3(0, footYOffset, 0);
                    inIdlePosition = true;
                }
            }
            transform.rotation = Quaternion.LookRotation(body.forward, info.normal) * Quaternion.Euler(footRotationOffset);
        }

        MoveTheFoot();
    }

    void MoveTheFoot()
    {
        if (lerp < 1)
        {
            Vector3 footPosition = Vector3.Slerp(oldPosition, newPosition, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;
            lerp += Time.deltaTime * speed;

            if (lerp >= 1f)
            {
                currentPosition = newPosition;
                oldPosition = newPosition;
                HasSteppedLast = true;
                otherFoot.HasSteppedLast = false;

                if (stepclip != null && audioSource != null)
                {
                    audioSource.PlayOneShot(stepclip);
                }
            }
        }
        else
        {
            oldPosition = newPosition;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterTail : MonoBehaviour
{
    [SerializeField] private Transform tailTarget;

    [SerializeField] private float tailLength;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float surfaceOffset;

    [SerializeField] private Transform parent;
    
    void LateUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, tailLength))
        {
            float _proximity = 1f - Mathf.InverseLerp(0f, tailLength, hit.distance);
            float _dynamicSideOffset = tailLength * _proximity;

            Vector3 incomingDir = -ray.direction;
            Vector3 surfaceRight = Vector3.Cross(hit.normal, Vector3.up).normalized * _dynamicSideOffset;
            float side = Vector3.Dot(incomingDir, surfaceRight);

            Vector3 targetPosition;

            if (side < 0f)
            {
                targetPosition = hit.point + surfaceRight + hit.normal * surfaceOffset * _dynamicSideOffset;
            }
            else
            {
                targetPosition = hit.point - surfaceRight + hit.normal * surfaceOffset * _dynamicSideOffset;
            }
            tailTarget.position = Vector3.Lerp(tailTarget.position, targetPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            tailTarget.localPosition = Vector3.Lerp(tailTarget.localPosition, parent.localPosition + new Vector3(0, 0, tailLength), Time.deltaTime * moveSpeed);
        }
    }
}
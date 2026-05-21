using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereCameraLook : MonoBehaviour
{
    [SerializeField] private Transform constrainedObject;
    [SerializeField] private Transform Camera;
    [SerializeField] private Vector3 offset;

    void Update()
    {
        Ray ray = new Ray(Camera.position, Camera.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Vector3 _target = new Vector3(hit.point.x + offset.x, hit.point.y + offset.y);
            constrainedObject.LookAt(hit.point);
        }
    }
}

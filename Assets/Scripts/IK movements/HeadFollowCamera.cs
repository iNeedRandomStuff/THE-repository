using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollowCamera : MonoBehaviour
{
    public GameObject xrCamera;
    public GameObject HeadIK;

    void Update()
    {
        if(xrCamera.activeInHierarchy == false)
        {
            HeadIK.SetActive(false);
            gameObject.GetComponent<HeadFollowCamera>().enabled = false;
        }
    }

    void LateUpdate()
    {
        transform.rotation = xrCamera.transform.rotation;
        transform.position = xrCamera.transform.position;
    }
}

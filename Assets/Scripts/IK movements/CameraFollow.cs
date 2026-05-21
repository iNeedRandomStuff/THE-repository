using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;

public class CameraFollow : NetworkBehaviour
{
    [SerializeField] private GameObject XRorigin;
    [SerializeField] private GameObject EmptyCamera;
    [SerializeField] private GameObject Head;

    private bool owner;
    private bool done;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            owner = true;
            //resetXRorigin();
            Head.gameObject.SetActive(false);
        }
        else
        {
            XRorigin.gameObject.SetActive(false);
        }
    }

    
    IEnumerator Start()
    {
        if(owner)
        {
            yield return new WaitForSeconds(0.2f);
            XRorigin.SetActive(false);
            yield return null;
            XRorigin.SetActive(true);
        }
    }

    void resetXRorigin()
    {
        if(!done)
        {
            XRorigin.SetActive(false);
            XRorigin.SetActive(true);
            done = true;
        }
    }


    void Update()
    {
        resetXRorigin();
        Vector3 _camPos = EmptyCamera.transform.position;
        _camPos.x = XRorigin.transform.position.x;
        _camPos.z = XRorigin.transform.position.z;
    }
}
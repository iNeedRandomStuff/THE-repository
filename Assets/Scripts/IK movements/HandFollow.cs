using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class HandFollow : NetworkBehaviour
{
    public Transform Controller;
    public bool isOwner = false;
    private Rigidbody rigidBody;

    [SerializeField] private GameObject netHand;
    [SerializeField] private float swayRange;

    public Quaternion offset;
    [HideInInspector]public float time;
    [HideInInspector] public float timeToReset;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            rigidBody = gameObject.GetComponent<Rigidbody>();
            isOwner = true;
        }
        else
        {
            gameObject.GetComponent<HandFollow>().enabled = false;
        }
    }

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        isOwner = true;
    }

    void FixedUpdate()
    {
        if (IsOwner)
        {
            physycsFollow();
        }
    }

    void physycsFollow()
    {
        print(offset);
        rigidBody.velocity = (Controller.position - transform.position) / Time.fixedDeltaTime;

        Quaternion rotationDifference = Controller.rotation * offset * Quaternion.Inverse(transform.rotation);
        if(offset != Quaternion.Euler(0, 0, 0))
        {
            time = time + 1f;
            if(time >= timeToReset)
            {
                offset = Quaternion.Euler(0, 0, 0);
                time = 0f;
            }
        }
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;

        rigidBody.angularVelocity = (rotationDifferenceInDegree);

        netHand.transform.position = transform.position;
        netHand.transform.rotation = transform.rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllignCharachterController : MonoBehaviour
{
    [SerializeField] private Transform Camera;

    private CharacterController controller;
    private BoxCollider box;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        box = GetComponent<BoxCollider>();
    }

    void Update()
    {
        Vector3 _CCcenter = controller.center;
        _CCcenter.x = Camera.localPosition.x;
        _CCcenter.z = Camera.localPosition.z;
        controller.center = _CCcenter;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEngine.XR;

public class ScaleAndAllignment : NetworkBehaviour
{
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHandIdlePos;
    [SerializeField] private Transform leftHandIdlePos;
    [SerializeField] private Transform body;
    [SerializeField] private Transform EmptyCamera;
    [SerializeField] private Transform avatar;
    [SerializeField] private GameObject XrOrigin;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float CameraToHeadOffset;

    private bool calibrationDone;
    private float playerHeight;
    public bool playerIsThere;
    public bool calibratedToVr;
    private InputDevice headset;

    // in unity units (meters)
    [SerializeField] private float AvatarHeight;

    [Header("Arms")]
    public GameObject LeftArm;
    public GameObject RightArm;
    public GameObject Head;

    Vector3 previousPosition;
    Vector3 movementDirection;

    [SerializeField] private PCmovement PCmovementScript;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            gameObject.GetComponent<ScaleAndAllignment>().enabled = false;
        }
    }

    
    IEnumerator Start()
    {
        yield return new WaitUntil(() => EmptyCamera.localPosition.y > 0.1f);
        //CharachterCalibration();
        headset = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        if (headset.isValid)
        {
            if (calibrationDone == false)
            {
                CharachterCalibration();
                print("calibrated to VR");
            }
        }
        else
        {
            if (calibrationDone == false)
            {
                PcCharachterCalibration();
                print("calibrated to PC");
            }
        }
    }

    void Update()
    {
        if (headset.TryGetFeatureValue(CommonUsages.userPresence, out playerIsThere) && headset.isValid)
        {
            if (!playerIsThere)
            {
                idlePose();
            }
        }

        if (calibrationDone == true)
        {
            AlignAvatarToCamera();


            /*
            if (calibratedToVr == true && playerIsThere == false)
            {
                idlePose();
            }
            */
        }
        RotateTheBody();
    }

    void RotateTheBody()
    {
        Vector3 _headForward = EmptyCamera.forward;

        _headForward.y = 0;
        _headForward.Normalize();

        Quaternion _targetRot = Quaternion.LookRotation(_headForward, Vector3.up);
        float _angleDiff = Quaternion.Angle(body.rotation, _targetRot);
        if (_angleDiff > 25f)
        {
            body.rotation = Quaternion.Slerp(body.rotation, _targetRot, Time.deltaTime * rotationSpeed);
        }
    }

    void CharachterCalibration()
    {
        playerHeight = EmptyCamera.transform.localPosition.y;
        print("player height: " + playerHeight);
        print("EmptyCamera.transform.localPosition.y " + EmptyCamera.transform.localPosition.y);
        float _scale = playerHeight / AvatarHeight * 1f;
        avatar.localScale = Vector3.one * _scale;
        PCmovementScript.enabled = false;
        calibrationDone = true;
        calibratedToVr = true;
    }

    void PcCharachterCalibration()
    {
        playerHeight = AvatarHeight * 0.9f;
        float _scale = playerHeight / AvatarHeight * 0.7f;
        avatar.localScale = Vector3.one * _scale;
        calibrationDone = true;
    }

    void AlignAvatarToCamera()
    {
        Vector3 _EmptyCamera = EmptyCamera.transform.position;
        Vector3 _newAvatarPos = avatar.position;
        _newAvatarPos.x = _EmptyCamera.x;
        _newAvatarPos.z = _EmptyCamera.z;
        _newAvatarPos.y = _EmptyCamera.y - CameraToHeadOffset;
        avatar.position = Vector3.Lerp(avatar.position, _newAvatarPos, Time.deltaTime * 100f);
    }

    void idlePose()
    {
        //rightHand = rightHandIdlePos;
        //leftHand = leftHandIdlePos;
        EmptyCamera.localPosition = new Vector3(0f, playerHeight, 0f);
    }
}
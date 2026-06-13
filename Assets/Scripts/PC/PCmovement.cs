using FishNet.Example.Scened;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.UI;

public class PCmovement : NetworkBehaviour
{
    public static PCmovement Local;

    [Header("Base setup")]
    public float walkingSpeed = 9f;
    public float gravity = 20.0f;
    public float lookSpeed;
    public float lookXLimit = 45.0f;
    public Light light;
    public CharacterController characterController;
    public Transform player;
    public float turnAngle;
    public float baseAngleY;
    public bool isLizzardMonster;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0f;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Transform playerModel;
    [SerializeField] private GameObject Canvas;
    [SerializeField] private Slider sensSlider;

    public void changeSensetivity(float sensetivity)
    {
        lookSpeed = sensetivity;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            if (light != null)
            {
                light.enabled = true;
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Local = this;
        }
        else
        {
            if (Canvas != null)
            {
                Canvas.SetActive(false);
            }
            playerCamera.SetActive(false);
            gameObject.GetComponent<PCmovement>().enabled = false;
        }
    }

    void Start()
    {
        canMove = true;
    }

    void Update()
    {
        if (sensSlider != null)
        {
            sensSlider.onValueChanged.AddListener((v) => { lookSpeed = v; });
        }
        else
        {
            GameObject _slider = GameObject.FindGameObjectWithTag("mouseSensSlider");
            if (_slider != null)
            {
                sensSlider = _slider.GetComponent<Slider>();
            }
        }

        float horizontalAngleAbberation = turnAngle * Input.GetAxis("Horizontal");
        Quaternion _offsetRotation = Quaternion.Euler(0, horizontalAngleAbberation, 0);
        Vector3 forward = _offsetRotation * player.forward;
        Vector3 right = player.TransformDirection(Vector3.right);

        if (canMove == true)
        {
            float curSpeedX = walkingSpeed * Input.GetAxis("Vertical");
            float curSpeedY = walkingSpeed * Input.GetAxis("Horizontal");
            float movementDirectionY = moveDirection.y;
            if (isLizzardMonster == true)
            {
                moveDirection = (forward * curSpeedX);
            }
            else
            {
                moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            }
            moveDirection.y = movementDirectionY;
            characterController.Move(moveDirection * Time.deltaTime);
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (playerCamera != null)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                player.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
                if (isLizzardMonster)
                {
                    playerModel.localRotation = Quaternion.Euler(0, baseAngleY + playerModel.localRotation.y + horizontalAngleAbberation, 0);
                }
            }
        }
    }
}
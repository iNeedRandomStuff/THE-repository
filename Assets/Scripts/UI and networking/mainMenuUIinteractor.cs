using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainMenuUIinteractor : MonoBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 9f;
    public float gravity = 20.0f;
    public float lookSpeed;
    public float lookXLimit = 45.0f;
    public CharacterController characterController;
    public Transform player;
    public float turnAngle;
    public float baseAngleY;
    public bool isPC;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0f;

    [SerializeField] private LineRenderer lr;
    [SerializeField] private GameObject controllerR;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Transform playerModel;
    [SerializeField] private GameObject Canvas;
    [SerializeField] private Slider sensSlider;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lr.positionCount = 2;
    }

    void Update()
    {
        movement();
        Interact();
    }

    void movement()
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
            if (isPC == true)
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
                if (isPC)
                {
                    playerModel.localRotation = Quaternion.Euler(0, baseAngleY + playerModel.localRotation.y + horizontalAngleAbberation, 0);
                }
            }
        }
    }

    void Interact()
    {
        Ray ray = new Ray(controllerR.transform.position, controllerR.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            
            lr.SetPosition(0, ray.origin);
            lr.SetPosition(1, hit.point);
            
        }
        else
        {
            
            lr.SetPosition(0, ray.origin);
            lr.SetPosition(1, ray.origin + ray.direction * 100f);
            
        }
    }
}

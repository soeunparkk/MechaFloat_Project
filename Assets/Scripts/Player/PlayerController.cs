using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10f;

    [Header("Camera Settings")]
    public Camera thirdPersonCamera;
    public float cameraDistance = 5.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 10.0f;
    public float mouseSensitivity = 200f;

    [Header("Mouse Settings")]
    private float CurrentX = 0.0f;
    private float CurrentY = 45.0f;
    private const float Y_ANGLE_MIN = -50.0f;
    private const float Y_ANGLE_MAX = 80.0f;

    [Header("Component")]
    private Rigidbody rb;
    private PlayerJump playerJump;
    private AchievementConditionChecker checker;

    [NonSerialized]
    public PlayerPickup playerPickup;
    public bool HasBalloon { get; private set; } = false;
    public BalloonController balloon;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerJump = GetComponent<PlayerJump>();
        playerPickup = GetComponent<PlayerPickup>();
        checker = GetComponent<AchievementConditionChecker>();

        Cursor.lockState = CursorLockMode.Locked;
        thirdPersonCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        HandleRotation();

        checker.CheckHeightAchievement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerJump.HandleJump();
            checker.OnJumpPerformed();
            
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerPickup.HandleEquipmentToggle();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        playerJump.ApplyBuoyancy();
    }

    public void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        CurrentX += mouseX;
        CurrentY -= mouseY;
        CurrentY = Mathf.Clamp(CurrentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        // 카메라 위치 계산
        Vector3 dir = new Vector3(0, 0, -cameraDistance);
        Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0);
        thirdPersonCamera.transform.position = transform.position + rotation * dir;
        thirdPersonCamera.transform.LookAt(transform.position);

        // 마우스 휠 줌 처리
        cameraDistance = Mathf.Clamp(
            cameraDistance - Input.GetAxis("Mouse ScrollWheel") * 5,
            minDistance,
            maxDistance
        );
    }

    public void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 카메라 기준 이동 벡터 계산
        Vector3 cameraForward = thirdPersonCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = thirdPersonCamera.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 movement = cameraForward * moveVertical + cameraRight * moveHorizontal;

        // 캐릭터 회전 처리
        if (movement.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                toRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }

    public bool BalloonController
    {
        get { return balloon != null; } 
    }
    public void PickupBalloon()
    {
        HasBalloon = true;
        Debug.Log("헬륨 풍선 장착됨 - HasBalloon = true");
    }

    public void DropBalloon()
    {
        HasBalloon = false;
        Debug.Log("풍선 해제됨 - HasBalloon = false");
    }
}
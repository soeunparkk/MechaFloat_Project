using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float rotationSpeed = 10f;

    [Header("Camera Settings")]
    public Camera thirdPersonCamera;
    public float cameraDistance = 5.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 10.0f;
    public float mouseSensitivity = 200f;

    private float CurrentX = 0.0f;
    private float CurrentY = 45.0f;
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    private Rigidbody rb;
    private bool canJump = true;
    public float fallingThreshold = -0.1f;

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;
    public const int groundCheckPoints = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        thirdPersonCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        HandleRotation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
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

    void HandleJump()
    {
        if (isGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
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

    public bool isFalling()
    {
        return rb.velocity.y < fallingThreshold && !isGrounded();
    }

    public bool isGrounded()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, 1.0f);
        if (grounded)
        {
            canJump = true;
        }
        return grounded;
    }

    public float GetVerticalVelocity()
    {
        return rb.velocity.y;
    }
}
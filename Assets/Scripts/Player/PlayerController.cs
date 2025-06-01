using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float runSpeed = 7.0f;
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
    [NonSerialized]
    public PlayerPickup playerPickup;

    [Header("Wind")]
    private float originalMass;
    public float balloonMass = 0.5f;
    public float gravityLightDuration = 10f;
    private float balloonTimer = 0f;
    private bool isBalloonEffectActive = false;
    public bool HasBalloon { get; private set; } = false;
    public BalloonController balloon;

    [Header("Footstep Sound")]
    public AudioSource footstepAudioSource;
    public AudioClip walkClip;
    public AudioClip runClip;
    public float walkSpeedThreshold = 2.0f;
    public float runSpeedThreshold = 5.0f;
    private float footstepTimer = 0f;
    private float walkInterval = 0.6f;
    private float runInterval = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerJump = GetComponent<PlayerJump>();
        playerPickup = GetComponent<PlayerPickup>();

        Cursor.lockState = CursorLockMode.Locked;
        thirdPersonCamera.gameObject.SetActive(true);
        originalMass = rb.mass;
    }

    void Update()
    {
        HandleRotation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerJump.HandleJump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerPickup.HandleEquipmentToggle();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (isBalloonEffectActive)
        {
            balloonTimer += Time.deltaTime;

            if (balloonTimer >= gravityLightDuration)
            {
                rb.mass = originalMass;
                isBalloonEffectActive = false;
                Debug.Log("풍선 효과 종료 - 중력 원래대로 복구");
            }
        }

        HandleFootstepSound();
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

        Vector3 dir = new Vector3(0, 0, -cameraDistance);
        Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0);
        thirdPersonCamera.transform.position = transform.position + rotation * dir;
        thirdPersonCamera.transform.LookAt(transform.position);

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

        Vector3 cameraForward = thirdPersonCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = thirdPersonCamera.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 movement = cameraForward * moveVertical + cameraRight * moveHorizontal;

        if (movement.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                toRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
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

    public bool IsMoving
    {
        get
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            return Mathf.Abs(moveHorizontal) > 0.1f || Mathf.Abs(moveVertical) > 0.1f;
        }
    }

    private void HandleFootstepSound()
    {
        if (!IsMoving || !thirdPersonCamera.gameObject.activeSelf)
        {
            footstepTimer = 0f;
            return;
        }

        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            float speed = rb.velocity.magnitude;

            if (speed < walkSpeedThreshold)
            {
                footstepAudioSource.clip = walkClip;
                footstepTimer = walkInterval;
            }
            else
            {
                footstepAudioSource.clip = runClip;
                footstepTimer = runInterval;
            }

            footstepAudioSource.Play();
        }
    }

    public void Knockback(Vector3 forceDirection, float forceStrength)
    {
        rb.velocity = Vector3.zero; // 기존 속도 제거
        rb.AddForce(forceDirection.normalized * forceStrength, ForceMode.Impulse);
    }
}

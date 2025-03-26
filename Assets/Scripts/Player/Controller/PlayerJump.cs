using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Player Jump")]
    public float jumpForce = 5.0f;
    public float fallingThreshold = -0.1f;

    [Header("Physics")]
    public float liftForce = 2.0f;

    private Rigidbody rb;
    private PlayerPickup playerPickup;

    private bool canJump = true;
    private bool isZeroGravity = false;                 // 무중력 상태 여부

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;                     // 등반 가능한 최대 경사 각도
    public const int groundCheckPoints = 5;             // 지면 체크 포인트 수

    [Header("Gravity Settings")]
    public float spaceGravity = -0.1f;                  // 우주 공간에서의 가짜 중력
    public float slowDownFactor = 0.5f;                 // 점프 후 감속 비율
    public float maxRiseSpeed = 5.0f;                   // 최대 상승 속도
    public float maxFallSpeed = -2.0f;                  // 최대 하강 속도
    public float zeroGravityJumpForce = 3.0f;           // 우주에서의 점프력

    private float normalGravity = -9.81f;
    private float buoyancyGravityFactor = 0.5f;

    private Vector3 defaultGravity;                     // 기본 중력 값 저장

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPickup = GetComponent<PlayerPickup>();

        defaultGravity = Physics.gravity;
    }

    void FixedUpdate()
    {
        if (isZeroGravity)
        {
            // 우주 구역: 가짜 중력 적용
            rb.velocity += Vector3.up * spaceGravity * Time.fixedDeltaTime;

            // 점프 후 감속 및 속도 제한
            if (rb.velocity.y > 0)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * slowDownFactor);
            }

            // 최대 상승 속도 제한
            if (rb.velocity.y > maxRiseSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, maxRiseSpeed, rb.velocity.z);
            }

            // 최대 하강 속도 제한
            if (rb.velocity.y < maxFallSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, maxFallSpeed, rb.velocity.z);
            }
        }
        else
        {
            // 일반 구역: 기본 중력 사용
            Physics.gravity = defaultGravity;
        }

        ApplyBuoyancyEffect();
    }

    public void HandleJump()
    {
        if (isGrounded())
        {
            if (isZeroGravity)
            {
                rb.velocity = new Vector3(rb.velocity.x, zeroGravityJumpForce, rb.velocity.z);
            }
            else
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            canJump = false;
        }
    }

    private void ApplyBuoyancyEffect()
    {
        ItemSO balloonData = playerPickup.GetCurrentBalloonData();

        if (balloonData != null && balloonData.isBuoyancy)
        {
            // 부력이 적용되면 중력을 줄임
            Physics.gravity = new Vector3(0, normalGravity * buoyancyGravityFactor, 0);
        }
        else
        {
            Physics.gravity = defaultGravity;
        }
    }

    public void ApplyBuoyancy()
    {
        ItemSO balloonData = playerPickup.GetCurrentBalloonData();

        if (balloonData != null && balloonData.isBuoyancy)
        {
            rb.AddForce(Vector3.up * liftForce, ForceMode.Acceleration);
        }
    }

    public bool isFalling()
    {
        return rb.velocity.y < fallingThreshold && !isGrounded();
    }

    public bool isGrounded()
    {
        // 지면 체크 로직 (우주 공간에서도 동일하게 적용)
        RaycastHit hit;
        bool grounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f);

        if (grounded)
        {
            // 우주 공간 내의 발판을 밟았는지 확인
            if (isZeroGravity && hit.collider.CompareTag("SpaceGround"))
            {
                canJump = true;
                return true;
            }
            // 일반 지면을 밟았는지 확인
            else if (!isZeroGravity)
            {
                canJump = true;
                return true;
            }
        }

        return false;
    }

    public void SetGravityState(bool zeroGravity)
    {
        isZeroGravity = zeroGravity;
        rb.useGravity = !zeroGravity;           // 무중력 상태면 중력 OFF

        if (zeroGravity)
        {
            rb.velocity = Vector3.zero;         // 무중력 상태에서 바로 속도를 리셋
            Physics.gravity = Vector3.zero;     // 중력 제거
        }
        else
        {
            Physics.gravity = defaultGravity;   // 기본 중력 복원
        }
    }

    public float GetVerticalVelocity()
    {
        return rb.velocity.y;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpaceZone"))
        {
            SetGravityState(true);              // 우주에 들어가면 무중력
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SpaceZone"))
        {
            SetGravityState(false);             // 우주에서 나오면 중력 다시 적용
        }
    }
}
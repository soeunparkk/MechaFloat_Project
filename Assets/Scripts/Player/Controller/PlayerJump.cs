using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Player Jump")]
    public float jumpForce = 5.0f;
    public float fallingThreshold = -0.1f;

    private Rigidbody rb;
    private PlayerPickup playerPickup;

    private bool canJump = true;
    public bool isZeroGravity = false;                 // 무중력 상태 여부

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;                     // 등반 가능한 최대 경사 각도
    public const int groundCheckPoints = 5;             // 지면 체크 포인트 수

    [Header("Gravity Setting")]
    [Header("Space Gravity")]
    public float slowDownFactor = 0.5f;                 // 점프 후 감속 비율
    public float zeroGravityJumpForce = 3.0f;           // 우주에서의 점프력
    [Header("All Gravity")]
    public float maxRiseSpeed;                          // 최대 상승 속도
    public float maxFallSpeed;                          // 최대 하강 속도

    private float normalGravity = -9.81f;
    private float buoyancyGravityFactor = 0.5f;

    private Vector3 defaultGravity;                     // 기본 중력 값 저장

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPickup = GetComponent<PlayerPickup>();

        if (!isZeroGravity)
        {
            defaultGravity = Physics.gravity;
        }
    }

    void FixedUpdate()
    {
        ItemSO balloonData = playerPickup.GetCurrentBalloonData();

        if (isZeroGravity)
        {
            StageSO currentStage = MapCheckingManager.instance.currentStageSO;

            // 우주 구역: 가짜 중력 적용
            rb.velocity += Vector3.up * currentStage.gravity * Time.fixedDeltaTime;

            // 점프 후 감속
            if (rb.velocity.y > 0)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * slowDownFactor);
            }

            // 풍선이 있다면 SO 기준 속도 제한, 없으면 기본값
            float riseLimit = (balloonData != null && balloonData.isBuoyancy) ? balloonData.maxRiseSpeed : maxRiseSpeed;
            float fallLimit = (balloonData != null && balloonData.isBuoyancy) ? balloonData.maxFallSpeed : maxFallSpeed;

            rb.velocity = new Vector3(
                rb.velocity.x,
                Mathf.Clamp(rb.velocity.y, fallLimit, riseLimit),
                rb.velocity.z
            );
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
            // 중력 조절
            Physics.gravity = new Vector3(0, normalGravity * balloonData.gravityScale, 0);

            // 부력 적용
            rb.AddForce(Vector3.up * balloonData.buoyancyForce, ForceMode.Acceleration);

            // 상승/하강 속도 제한 적용
            float clampedY = Mathf.Clamp(rb.velocity.y, balloonData.maxFallSpeed, balloonData.maxRiseSpeed);
            rb.velocity = new Vector3(rb.velocity.x, clampedY, rb.velocity.z);
        }
        else
        {
            Physics.gravity = defaultGravity;

            // 기본 속도 제한
            float clampedY = Mathf.Clamp(rb.velocity.y, maxFallSpeed, maxRiseSpeed);
            rb.velocity = new Vector3(rb.velocity.x, clampedY, rb.velocity.z);
        }
    }



    public void ApplyBuoyancy()
    {
        ItemSO balloonData = playerPickup.GetCurrentBalloonData();

        if (balloonData != null && balloonData.isBuoyancy)
        {
            rb.AddForce(Vector3.up * balloonData.buoyancyForce, ForceMode.Acceleration);
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
            if (isZeroGravity && hit.collider.CompareTag("Ground"))
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
        rb.useGravity = !zeroGravity;

        if (zeroGravity)
        {
            rb.velocity = Vector3.zero;
            Physics.gravity = Vector3.zero;
        }
        else
        {
            StageSO currentStage = MapCheckingManager.instance.currentStageSO;

            // StageSO의 gravity를 Physics.gravity에 반영
            float gravityValue = currentStage != null ? currentStage.gravity : -9.81f;
            Vector3 newGravity = new Vector3(0, gravityValue, 0);

            Physics.gravity = newGravity;
            defaultGravity = newGravity; // 기본 중력값도 갱신
        }
    }

    public float GetVerticalVelocity()
    {
        return rb.velocity.y;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    #region Fields

    [Header("Jump Settings")]
    public float jumpForce = 5.0f;                          // 점프 힘
    public float fallingThreshold = -0.1f;                  // 낙하 판단 기준 속도
    public int jumpCount = 0;                               // 점프 횟수 카운트

    private Rigidbody rb;
    private PlayerPickup playerPickup;
    private bool canJump = true;
    public bool isZeroGravity = false;                      // 무중력 상태 여부

    [Header("Ground Detection")]
    public float groundCheckDistance = 0.3f;                // 지면 체크 거리
    public float slopedLimit = 45f;                         // 허용 경사도 제한
    public const int groundCheckPoints = 5;                 // 착지 검사 포인트 수
    [SerializeField] private LayerMask groundLayerMask;     // 착지 가능 레이어

    [Header("Gravity Settings")]
    public float slowDownFactor = 0.5f;                     // 무중력 점프 후 감속 비율
    public float zeroGravityJumpForce = 3.0f;               // 우주 구역 전용 점프 힘
    public float maxRiseSpeed;                              // 최대 상승 속도 제한
    public float maxFallSpeed;                              // 최대 하강 속도 제한

    private float normalGravity = -9.81f;                   // 기본 중력 값
    private Vector3 defaultGravity;                         // 기본 중력 벡터 저장

    [Header("Coyote Time (Ground Forgiveness)")]
    public float coyoteTime = 0.15f;                        // 코요테 타임 (지면 관성 시간)
    private float coyoteTimeCounter;                        // 코요테 타이머

    [Header("Better Jump Settings")]
    public float fallMultiplier = 2.5f;                     // 하강 중 중력 강화 배율
    public float lowJumpMultiplier = 2.0f;                  // 짧은 점프 중 중력 강화 배율

    private float defaultJumpForce = 7f;
    private float currentJumpForce;

    #endregion

    #region Unity Methods

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPickup = GetComponent<PlayerPickup>();
        currentJumpForce = defaultJumpForce;


        if (!isZeroGravity)
        {
            defaultGravity = Physics.gravity;
        }
    }

    private void FixedUpdate()
    {
        UpdateCoyoteTime();

        BalloonController equippedBalloon = InventoryManager.Instance.GetSelectedBalloon();
        ItemSO balloonData = equippedBalloon != null ? equippedBalloon.balloonData : null;
        bool hasBuoyantBalloon = balloonData != null && balloonData.isBuoyancy;

        if (isZeroGravity)
        {
            StageSO currentStage = MapCheckingManager.instance.currentStageSO;
            rb.velocity += Vector3.up * currentStage.gravity * Time.fixedDeltaTime;

            if (rb.velocity.y > 0)
            {
                float slowDownRate = 0.2f;
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * (1 - slowDownRate * Time.fixedDeltaTime), rb.velocity.z);
            }

            float riseLimit = hasBuoyantBalloon ? balloonData.maxRiseSpeed : maxRiseSpeed;
            float fallLimit = hasBuoyantBalloon ? balloonData.maxFallSpeed : maxFallSpeed;

            rb.velocity = new Vector3(
                rb.velocity.x,
                Mathf.Clamp(rb.velocity.y, fallLimit, riseLimit),
                rb.velocity.z
            );
        }

        // 점프 높이 조절 (풍선 없을 때만)
        if (!isZeroGravity && !hasBuoyantBalloon)
        {
            if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
            }
            else if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
            }
        }

        // 높이 업적 체크
        float currentHeight = transform.position.y;
        AchievementConditionChecker checker = GetComponent<AchievementConditionChecker>();
        if (checker != null)
        {
            checker.CheckHeightAchievement(currentHeight);
        }

        ApplyBuoyancyEffect();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = transform.position + Vector3.up * 0.1f;
        Gizmos.DrawWireSphere(center + Vector3.down * groundCheckDistance, 0.2f);
    }

    #endregion

    #region Jump Logic

    public void HandleJump()
    {
        if (coyoteTimeCounter > 0f)
        {
            if (isZeroGravity)
            {
                rb.velocity = new Vector3(rb.velocity.x, zeroGravityJumpForce, rb.velocity.z);
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            }

            canJump = false;
            jumpCount++;

            AchievementConditionChecker checker = GetComponent<AchievementConditionChecker>();
            if (checker != null)
            {
                checker.OnJumpPerformed(jumpCount);
            }

            coyoteTimeCounter = 0f;
        }
    }

    private void UpdateCoyoteTime()
    {
        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    public bool isGrounded()
    {
        Vector3[] checkPoints = new Vector3[groundCheckPoints];
        Vector3 center = transform.position + Vector3.down * 0.1f;

        checkPoints[0] = center;
        checkPoints[1] = center + transform.forward * 0.3f;
        checkPoints[2] = center - transform.forward * 0.3f;
        checkPoints[3] = center + transform.right * 0.3f;
        checkPoints[4] = center - transform.right * 0.3f;

        for (int i = 0; i < groundCheckPoints; i++)
        {
            if (Physics.Raycast(checkPoints[i], Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayerMask))
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

                if (slopeAngle <= slopedLimit)
                {
                    canJump = true;
                    return true;
                }
            }
        }

        return false;
    }

    public bool isFalling()
    {
        return rb.velocity.y < fallingThreshold && !isGrounded();
    }

    #endregion

    #region Gravity Control

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
            float gravityValue = currentStage != null ? currentStage.gravity : -9.81f;
            Vector3 newGravity = new Vector3(0, gravityValue, 0);

            Physics.gravity = newGravity;
            defaultGravity = newGravity;
        }
    }

    private void ApplyBuoyancyEffect()
    {
        BalloonController equippedBalloon = InventoryManager.Instance.GetEquippedBalloon();
        ItemSO balloonData = equippedBalloon != null ? equippedBalloon.balloonData : null;

        if (balloonData != null && balloonData.isBuoyancy)
        {
            // 풍선용 중력 설정
            Physics.gravity = new Vector3(0, normalGravity * balloonData.gravityScale, 0);

            // 부력 적용
            rb.AddForce(Vector3.up * balloonData.buoyancyForce, ForceMode.Acceleration);

            // 하강 속도만 제한 (너무 빠르게 떨어지지 않도록)
            if (rb.velocity.y < 0)
            {
                float clampedFall = Mathf.Max(rb.velocity.y, balloonData.maxFallSpeed);
                rb.velocity = new Vector3(rb.velocity.x, clampedFall, rb.velocity.z);
            }
        }
        else
        {
            // 일반 중력 복구
            Physics.gravity = defaultGravity;

            // 일반 상태에서 전체 속도 제한
            float clampedY = Mathf.Clamp(rb.velocity.y, maxFallSpeed, maxRiseSpeed);
            rb.velocity = new Vector3(rb.velocity.x, clampedY, rb.velocity.z);
        }
    }

    public void ApplyBuoyancy()
    {
        if (InventoryManager.Instance != null)
        {
            BalloonController equippedBalloon = InventoryManager.Instance.GetEquippedBalloon();

            if (equippedBalloon != null && equippedBalloon.balloonData != null && equippedBalloon.balloonData.isBuoyancy)
            {
                rb.AddForce(Vector3.up * equippedBalloon.balloonData.buoyancyForce, ForceMode.Acceleration);
            }
            else
            {
                Debug.LogWarning("No balloon equipped!");
            }
        }
        else
        {
            Debug.LogError("InventoryManager.Instance is null!");
        }
    }

    public float GetVerticalVelocity()
    {
        return rb.velocity.y;
    }

    public void SetJumpForce(float newForce)
    {
        currentJumpForce = newForce;
    }
    public void ResetJumpForce()
    {
        currentJumpForce = defaultJumpForce;
    }

    #endregion
}
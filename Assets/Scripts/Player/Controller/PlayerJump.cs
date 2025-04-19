using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Player Jump")]
    public float jumpForce = 5.0f;
    public float fallingThreshold = -0.1f;
    public int jumpCount = 0;
    //public int fallCount = 0;       // 낙하는 추후에 할 예정 미리 선안만 해둔 상태

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
        BalloonController equippedBalloon = InventoryManager.Instance.GetSelectedBalloon();
        ItemSO balloonData = equippedBalloon != null ? equippedBalloon.balloonData : null;

        if (isZeroGravity)
        {
            StageSO currentStage = MapCheckingManager.instance.currentStageSO;

            // 우주 구역: 가짜 중력 적용
            rb.velocity += Vector3.up * currentStage.gravity * Time.fixedDeltaTime;

            // 점프 후 감속
            if (isZeroGravity && rb.velocity.y > 0)
            {
                float slowDownRate = 0.2f;
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * (1 - slowDownRate * Time.fixedDeltaTime), rb.velocity.z);
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

        float currentHeight = transform.position.y;

        AchievementConditionChecker checker = GetComponent<AchievementConditionChecker>();
        if (checker != null)
        {
            checker.CheckHeightAchievement(currentHeight);
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
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
            canJump = false;

            jumpCount++;

            // 업적 체크
            AchievementConditionChecker checker = GetComponent<AchievementConditionChecker>();
            if (checker != null)
            {
                checker.OnJumpPerformed(jumpCount);
            }
        }
    }

    private void ApplyBuoyancyEffect()
    {
        BalloonController equippedBalloon = InventoryManager.Instance.GetEquippedBalloon(); // ← 변경됨!
        ItemSO balloonData = equippedBalloon != null ? equippedBalloon.balloonData : null;

        if (balloonData != null && balloonData.isBuoyancy)
        {
            Physics.gravity = new Vector3(0, normalGravity * balloonData.gravityScale, 0);
            rb.AddForce(Vector3.up * balloonData.buoyancyForce, ForceMode.Acceleration);

            float clampedY = Mathf.Clamp(rb.velocity.y, balloonData.maxFallSpeed, balloonData.maxRiseSpeed);
            rb.velocity = new Vector3(rb.velocity.x, clampedY, rb.velocity.z);
        }
        else
        {
            Physics.gravity = defaultGravity;

            float clampedY = Mathf.Clamp(rb.velocity.y, maxFallSpeed, maxRiseSpeed);
            rb.velocity = new Vector3(rb.velocity.x, clampedY, rb.velocity.z);
        }
    }

    public void ApplyBuoyancy()
    {
        // 예외 방지용으로 null 체크를 추가
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

    public bool isFalling()
    {
        return rb.velocity.y < fallingThreshold && !isGrounded();
    }

    public bool isGrounded()
    {
        Vector3[] checkPoints = new Vector3[groundCheckPoints];
        Vector3 center = transform.position + Vector3.down * 0.1f; // pivot 기준 아래로

        checkPoints[0] = center;
        checkPoints[1] = center + transform.forward * 0.3f;
        checkPoints[2] = center - transform.forward * 0.3f;
        checkPoints[3] = center + transform.right * 0.3f;
        checkPoints[4] = center - transform.right * 0.3f;

        for (int i = 0; i < groundCheckPoints; i++)
        {
            if (Physics.Raycast(checkPoints[i], Vector3.down, out RaycastHit hit, 1.1f))
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

                if (slopeAngle <= slopedLimit)
                {
                    if (isZeroGravity)
                    {
                        if (hit.collider.CompareTag("Ground"))
                        {
                            canJump = true;
                            return true;
                        }
                    }
                    else
                    {
                        canJump = true;
                        return true;
                    }
                }
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

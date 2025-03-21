using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Player Jump")]
    public float jumpForce = 5.0f;
    public float fallingThreshold = -0.1f;

    private Rigidbody rb;

    private bool canJump = true;
    private bool isZeroGravity = false;                 // ���߷� ���� ����

    [Header("Ground Check Setting")]
    public float groundCheckDistance = 0.3f;
    public float slopedLimit = 45f;                     // ��� ������ �ִ� ��� ����
    public const int groundCheckPoints = 5;             // ���� üũ ����Ʈ ��

    [Header("Gravity Settings")]
    public float spaceGravity = -0.1f;                  // ���� ���������� ��¥ �߷�
    public float slowDownFactor = 0.5f;                 // ���� �� ���� ����
    public float maxRiseSpeed = 5.0f;                   // �ִ� ��� �ӵ�
    public float maxFallSpeed = -2.0f;                  // �ִ� �ϰ� �ӵ�
    public float zeroGravityJumpForce = 3.0f;           // ���ֿ����� ������

    private Vector3 defaultGravity;                     // �⺻ �߷� �� ����

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultGravity = Physics.gravity;
    }

    void FixedUpdate()
    {
        if (isZeroGravity)
        {
            // ���� ����: ��¥ �߷� ����
            rb.velocity += Vector3.up * spaceGravity * Time.fixedDeltaTime;

            // ���� �� ���� �� �ӵ� ����
            if (rb.velocity.y > 0)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * slowDownFactor);
            }

            // �ִ� ��� �ӵ� ����
            if (rb.velocity.y > maxRiseSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, maxRiseSpeed, rb.velocity.z);
            }

            // �ִ� �ϰ� �ӵ� ����
            if (rb.velocity.y < maxFallSpeed)
            {
                rb.velocity = new Vector3(rb.velocity.x, maxFallSpeed, rb.velocity.z);
            }
        }
        else
        {
            // �Ϲ� ����: �⺻ �߷� ���
            Physics.gravity = defaultGravity;
        }
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

    public bool isFalling()
    {
        return rb.velocity.y < fallingThreshold && !isGrounded();
    }

    public bool isGrounded()
    {
        // ���� üũ ���� (���� ���������� �����ϰ� ����)
        RaycastHit hit;
        bool grounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f);

        if (grounded)
        {
            // ���� ���� ���� ������ ��Ҵ��� Ȯ��
            if (isZeroGravity && hit.collider.CompareTag("SpaceGround"))
            {
                canJump = true;
                return true;
            }
            // �Ϲ� ������ ��Ҵ��� Ȯ��
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
        rb.useGravity = !zeroGravity;           // ���߷� ���¸� �߷� OFF

        if (zeroGravity)
        {
            rb.velocity = Vector3.zero;         // ���߷� ���¿��� �ٷ� �ӵ��� ����
            Physics.gravity = Vector3.zero;     // �߷� ����
        }
        else
        {
            Physics.gravity = defaultGravity;   // �⺻ �߷� ����
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
            SetGravityState(true);              // ���ֿ� ���� ���߷�
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SpaceZone"))
        {
            SetGravityState(false);             // ���ֿ��� ������ �߷� �ٽ� ����
        }
    }
}
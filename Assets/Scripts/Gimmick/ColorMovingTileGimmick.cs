using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ColorMovingTileGimmick : MonoBehaviour
{
    public enum MoveDirection
    {
        XAxis,
        ZAxis
    }

    [Header("이동 설정")]
    [SerializeField] private MoveDirection moveDirection = MoveDirection.XAxis;
    [SerializeField, Range(0.1f, 20f)] private float moveDistance = 5f;
    [SerializeField, Range(0.1f, 10f)] private float moveSpeed = 2f;

    private Rigidbody rb;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool shouldMove = false;
    private bool hasMoved = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // MovePosition을 위해 필요
    }

    private void Start()
    {
        startPos = rb.position;

        Vector3 offset = moveDirection == MoveDirection.XAxis
            ? Vector3.right * moveDistance
            : Vector3.forward * moveDistance;

        targetPos = startPos + offset;
    }

    private void FixedUpdate()
    {
        if (shouldMove && !hasMoved)
        {
            Vector3 nextPosition = Vector3.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(nextPosition);

            if (Vector3.Distance(rb.position, targetPos) < 0.01f)
            {
                rb.MovePosition(targetPos); // 정확히 위치 정렬
                hasMoved = true;
                shouldMove = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && !hasMoved)
        {
            shouldMove = true;
        }
    }
}

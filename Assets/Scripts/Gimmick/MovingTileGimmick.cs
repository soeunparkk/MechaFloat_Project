using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingTileGimmick : MonoBehaviour
{
    public enum MoveDirection
    {
        XAxis,
        YAxis,
        ZAxis
    }

    [Header("이동 설정")]
    [SerializeField] private MoveDirection moveDirection = MoveDirection.XAxis;
    [SerializeField, Range(0.1f, 10f)] private float moveDistance = 2f;
    [SerializeField, Range(0.1f, 10f)] private float moveSpeed = 2f;

    private Rigidbody rb;
    private Vector3 startPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // MovePosition 사용을 위한 설정
    }

    private void Start()
    {
        startPos = rb.position;
    }

    private void FixedUpdate()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        Vector3 targetOffset = Vector3.zero;

        switch (moveDirection)
        {
            case MoveDirection.XAxis:
                targetOffset = new Vector3(offset, 0f, 0f);
                break;
            case MoveDirection.YAxis:
                targetOffset = new Vector3(0f, offset, 0f);
                break;
            case MoveDirection.ZAxis:
                targetOffset = new Vector3(0f, 0f, offset);
                break;
        }

        Vector3 nextPosition = startPos + targetOffset;
        rb.MovePosition(nextPosition);
    }
}

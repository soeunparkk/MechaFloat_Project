using System.Collections;
using UnityEngine;

public class ColorMovingTileGimmick : MonoBehaviour
{
    public enum MoveDirection
    {
        XAxis, ZAxis
    }

    public MoveDirection moveDirection = MoveDirection.XAxis;

    [Header("Movement Settings")]
    public float moveDistance = 5f;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool shouldMove = false;
    private bool hasMoved = false;

    void Start()
    {
        startPos = transform.position;

        // 미리 목표 위치 계산
        Vector3 offset = moveDirection == MoveDirection.XAxis
            ? Vector3.right * moveDistance
            : Vector3.forward * moveDistance;

        targetPos = startPos + offset;
    }

    void Update()
    {
        if (shouldMove && !hasMoved)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;
                hasMoved = true;
                shouldMove = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && !hasMoved)
        {
            collision.transform.SetParent(transform);
            shouldMove = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}

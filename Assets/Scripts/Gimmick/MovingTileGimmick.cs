using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTileGimmick : MonoBehaviour
{
    public enum MoveDirection 
    { 
        XAxis,
        YAxis,
        ZAxis
    }

    public MoveDirection moveDirection = MoveDirection.XAxis;

    [Header("Movement Settings")]
    public float moveDistance = 2f;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 lastPos;

    void Start()
    {
        startPos = transform.position;
        lastPos = startPos;
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        Vector3 targetOffset = Vector3.zero;

        if (moveDirection == MoveDirection.XAxis)
            targetOffset = new Vector3(offset, 0, 0);
        else if (moveDirection == MoveDirection.ZAxis)
            targetOffset = new Vector3(0, 0, offset);
        else if (moveDirection == MoveDirection.YAxis)
            targetOffset = new Vector3(0, offset, 0);

        transform.position = startPos + targetOffset;
    }

    private void LateUpdate()
    {
        lastPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerController controller = collision.transform.GetComponent<PlayerController>();
            if (controller != null)
            {
                if (controller.IsMoving)
                {
                    if (collision.transform.parent == transform)
                        collision.transform.SetParent(null);
                }
                else
                {
                    if (collision.transform.parent != transform)
                        collision.transform.SetParent(transform);
                }
            }
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
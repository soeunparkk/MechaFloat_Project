using System.Collections;
using UnityEngine;

public class ElevatorGimmick : MonoBehaviour
{
    private Rigidbody elevatorRigidbody;

    [Header("�÷��̾� �±�")]
    [SerializeField] private string playerTag = "Player";

    [Header("��ǥ ��ġ")]
    [SerializeField] private Transform targetPosition;

    [Header("�̵� �ӵ�")]
    [SerializeField, Range(0.1f, 10f)] private float moveSpeed = 2f;

    private bool shouldMove = false;
    private bool isMoving = false;

    private void Start()
    {
        elevatorRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (shouldMove && !isMoving)
        {
            StartCoroutine(MoveElevator());
        }
    }

    private IEnumerator MoveElevator()
    {
        isMoving = true;
        shouldMove = false;

        Vector3 start = elevatorRigidbody.position;
        Vector3 end = targetPosition.position;

        while (Vector3.Distance(elevatorRigidbody.position, end) > 0.01f)
        {
            Vector3 nextPosition = Vector3.MoveTowards(
                elevatorRigidbody.position,
                end,
                moveSpeed * Time.deltaTime);

            elevatorRigidbody.MovePosition(nextPosition);

            yield return new WaitForFixedUpdate();
        }

        elevatorRigidbody.MovePosition(end);
        isMoving = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(playerTag)) return;

        if (!isMoving)
        {
            shouldMove = true;
        }
    }
}

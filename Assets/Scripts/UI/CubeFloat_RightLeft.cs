using UnityEngine;

public class CubeFloat_RightLeft : MonoBehaviour
{
    public float distance = 0.5f; // �պ� �Ÿ��� ����(���� �� ����)
    public float cycleTime = 2f;  // �պ� �ѹ�(�¡�����)�� �ɸ��� �ð�(��)

    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 targetPos;
    private bool movingLeft = true;
    private float speed;

    void Start()
    {
        leftPos = transform.position + Vector3.left * distance;
        rightPos = transform.position + Vector3.right * distance;
        targetPos = leftPos;
        speed = (2f * distance) / (cycleTime / 2f); // ���� �̵��� �ɸ��� �ð��� cycleTime/2
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            if (movingLeft)
            {
                movingLeft = false;
                targetPos = rightPos;
            }
            else
            {
                movingLeft = true;
                targetPos = leftPos;
            }
        }
    }
}
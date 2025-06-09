using UnityEngine;

public class CubeFloat_RightLeft : MonoBehaviour
{
    public float distance = 0.5f; // 왕복 거리의 절반(한쪽 끝 기준)
    public float cycleTime = 2f;  // 왕복 한번(좌→우→좌)에 걸리는 시간(초)

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
        speed = (2f * distance) / (cycleTime / 2f); // 한쪽 이동에 걸리는 시간은 cycleTime/2
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
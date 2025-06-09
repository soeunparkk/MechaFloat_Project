using UnityEngine;

public class CubeFloatLeftRight : MonoBehaviour
{
    public float distance = 0.5f; // 왕복 거리의 절반(한쪽 끝 기준)
    public float cycleTime = 2f;  // 왕복 한번(좌→우→좌)에 걸리는 시간(초)

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float speed = (2f * Mathf.PI) / cycleTime; // 주기 계산
        float newX = Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(startPos.x + newX, startPos.y, startPos.z);
    }
}
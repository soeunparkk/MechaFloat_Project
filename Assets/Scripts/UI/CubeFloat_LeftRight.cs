using UnityEngine;

public class CubeFloatLeftRight : MonoBehaviour
{
    public float distance = 0.5f; // �պ� �Ÿ��� ����(���� �� ����)
    public float cycleTime = 2f;  // �պ� �ѹ�(�¡�����)�� �ɸ��� �ð�(��)

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float speed = (2f * Mathf.PI) / cycleTime; // �ֱ� ���
        float newX = Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(startPos.x + newX, startPos.y, startPos.z);
    }
}
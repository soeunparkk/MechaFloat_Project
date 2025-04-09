using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveManager.Instance.SavePlayerPosition(other.transform.position);
            Debug.Log("���̺��� ����: ��ġ ���� �Ϸ�");
        }
    }
}

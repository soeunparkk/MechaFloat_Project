using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveManager.Instance.SavePlayerPosition(other.transform.position);
            Debug.Log("세이브존 도달: 위치 저장 완료");
        }
    }
}

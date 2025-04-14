using UnityEngine;

public class PiecePickup : MonoBehaviour
{
    public ItemSO itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PieceManager.Instance.AddPiece(itemData.itemType);
            Destroy(gameObject);
        }
    }
}

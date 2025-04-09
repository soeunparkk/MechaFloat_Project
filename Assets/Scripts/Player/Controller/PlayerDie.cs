using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    [SerializeField] private PlayerRespawn playerRespawn;

    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // 사망 시 처리 (애니메이션, 이펙트 등 추가 가능)
        Debug.Log("플레이어 사망");

        // 즉시 리스폰 처리
        Respawn();
    }

    private void Respawn()
    {
        isDead = false;
        playerRespawn.RespawnPlayer();
    }
}

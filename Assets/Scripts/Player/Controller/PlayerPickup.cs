using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private PlayerController playerController;

    public Animator animator;

    [Header("References")]
    [SerializeField] private Transform balloonPivot;
    [SerializeField] private float pickupRange = 2f;

    private void Update()
    {
        AutoPickupBalloon();
    }

    private void AutoPickupBalloon()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Balloon") &&
                hit.TryGetComponent(out BalloonController balloon) &&
                balloon.assignedSlot == -1) // 이미 인벤토리에 있는 풍선 제외
            {
                AttemptAddToInventory(balloon);
            }
        }
    }

    private void AttemptAddToInventory(BalloonController balloon)
    {
        bool success = InventoryManager.Instance.AddToInventory(balloon);
        if (success)
        {
            StoreBalloon(balloon);
        }
    }

    private void StoreBalloon(BalloonController balloon)
    {
        balloon.transform.SetParent(transform);
        balloon.transform.localPosition = Vector3.zero;
        balloon.gameObject.SetActive(false);
    }

    public void HandleEquipmentToggle()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            BalloonController selected = InventoryManager.Instance.GetSelectedBalloon();
            if (selected == null) return;

            if (InventoryManager.Instance.IsBalloonEquipped(selected.assignedSlot))
            {
                UnequipBalloon(selected);
            }
            else
            {
                EquipBalloon(selected);
            }
        }
    }

    private void EquipBalloon(BalloonController balloon)
    {
        balloon.owner = this;

        InventoryManager.Instance.UnequipCurrentBalloon();

        balloon.transform.SetParent(balloonPivot);
        balloon.transform.localPosition = Vector3.zero;
        balloon.gameObject.SetActive(true);
        balloon.StartDurabilityReduction();
        InventoryManager.Instance.UpdateHotbarUI(balloon.assignedSlot);

        if (balloon.TryGetComponent(out BalloonStateMachine balloonSM))
        {
            var playerJump = GetComponent<PlayerJump>();
            var playerController = GetComponent<PlayerController>();
            balloonSM.Init(playerJump, playerController);
        }   

        playerController.PickupBalloon();

        if (animator != null)
            animator.SetBool("HasBallon", true);
    }

    private void UnequipBalloon(BalloonController balloon)
    {
        balloon.transform.SetParent(transform);
        balloon.transform.localPosition = Vector3.zero;
        balloon.gameObject.SetActive(false);
        balloon.StopDurabilityReduction();
        InventoryManager.Instance.UpdateHotbarUI(balloon.assignedSlot);

        
        playerController.DropBalloon();

        if (animator != null)
            animator.SetBool("HasBallon", false);
    }


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
}
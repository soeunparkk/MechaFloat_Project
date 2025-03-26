using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState;
    public PlayerController PlayerController;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        TransitionToState(new IdleState(this));
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate();
        }
    }

    public void TransitionToState(PlayerState newState)
    {
        if (currentState?.GetType() == newState.GetType()) return;

        currentState?.Exit();

        currentState = newState;

        currentState.Enter();

        Debug.Log($"���� ��ȯ �Ǵ� ������Ʈ {newState.GetType().Name}");
    }
}
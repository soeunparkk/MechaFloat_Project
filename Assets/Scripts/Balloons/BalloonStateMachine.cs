using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonStateMachine : MonoBehaviour
{
    public BalloonState currentState;
    public BalloonController balloonController;
    public BalloonAnimationManager balloonAnimator;
    public PlayerJump playerJump;
    public PlayerController playerController;

    public void Init(PlayerJump jump, PlayerController controller)
    {
        playerJump = jump;
        playerController = controller;
    }

    private void Awake()
    {
        balloonController = GetComponent<BalloonController>();
        balloonAnimator = GetComponent<BalloonAnimationManager>();
    }

    private void Start()
    {
        TransitionToState(new B_IdleState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void TransitionToState(BalloonState newState)
    {
        if (currentState?.GetType() == newState.GetType()) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();

        Debug.Log($"[Balloon] 상태 전환 → {newState.GetType().Name}");
    }
}


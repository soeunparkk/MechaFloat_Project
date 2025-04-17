using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState 모든 플레이어 상태의 기본이 되는 추상 클래스
public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerController playerController;
    protected PlayerAnimationManager animationManager;
    protected PlayerJump playerJump;
    protected PlayerPickup playerPickup;

    public PlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.playerController = stateMachine.PlayerController;
        this.animationManager = stateMachine.GetComponent<PlayerAnimationManager>();
        this.playerJump = stateMachine.GetComponent<PlayerJump>();
        this.playerPickup = stateMachine.GetComponent<PlayerPickup>();
    }

    public virtual void Enter() { }             
    public virtual void Exit() { } 
    public virtual void Update() { }    
    public virtual void FixedUpdate() { }

    // 상태 전호나 조건을 체크하는 메서드
    protected void CheckTransitions()
    {
        if (playerJump.isGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                stateMachine.TransitionToState(new MovingState(stateMachine));
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                stateMachine.TransitionToState(new PickupState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new IdleState(stateMachine));
            }
        }
        else
        {
            // 공중에 있을대 상태 전환 로직
            if (playerJump.GetVerticalVelocity() > 0)        // 받아온 Y축 속도 값이 + 일때
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new FallingState(stateMachine));     // 받아온 Y축 속도 값이 - 일떼 [낙하 상태]
            }
        }
    }
}

public class IdleState : PlayerState
{
    public IdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();
    }
}

public class MovingState : PlayerState
{
    private bool isRunnig;
    public MovingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        // 현재 속도 크기 가져오기
        float speed = playerController.rb.velocity.magnitude;

        // 속도가 일정 값 이상이면 달리기 상태
        isRunnig = speed > 3.0f;

        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();
    }
}

public class JumpingState : PlayerState
{
    public JumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();
    }
}

public class FallingState : PlayerState
{
    public FallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();
    }
}

public class PickupState : PlayerState
{
    public PickupState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    { 
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();
    }
}
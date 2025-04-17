using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerState ��� �÷��̾� ������ �⺻�� �Ǵ� �߻� Ŭ����
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

    // ���� ��ȣ�� ������ üũ�ϴ� �޼���
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
            // ���߿� ������ ���� ��ȯ ����
            if (playerJump.GetVerticalVelocity() > 0)        // �޾ƿ� Y�� �ӵ� ���� + �϶�
            {
                stateMachine.TransitionToState(new JumpingState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new FallingState(stateMachine));     // �޾ƿ� Y�� �ӵ� ���� - �϶� [���� ����]
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
        // ���� �ӵ� ũ�� ��������
        float speed = playerController.rb.velocity.magnitude;

        // �ӵ��� ���� �� �̻��̸� �޸��� ����
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BalloonState
{
    protected BalloonStateMachine stateMachine;
    protected BalloonController balloonController;
    protected BalloonAnimationManager animator;
    protected PlayerJump playerJump;
    protected PlayerController playerController;

    public BalloonState(BalloonStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        this.balloonController = stateMachine.balloonController;
        this.animator = stateMachine.balloonAnimator;
        this.playerJump = stateMachine.playerJump;
        this.playerController = stateMachine.playerController;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }

    protected void CheckTransitions()
    {
        if (playerJump.isGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.TransitionToState(new B_JumpingState(stateMachine));
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                stateMachine.TransitionToState(new B_MovingState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new B_IdleState(stateMachine));
            }
        }
        else
        {
            // ���߿� ������ ���� ��ȯ ����
            if (playerJump.GetVerticalVelocity() > 0)        // �޾ƿ� Y�� �ӵ� ���� + �϶�
            {
                stateMachine.TransitionToState(new B_JumpingState(stateMachine));
            }
            else
            {
                stateMachine.TransitionToState(new B_FallingState(stateMachine));     // �޾ƿ� Y�� �ӵ� ���� - �϶� [���� ����]
            }
        }
    }
}

public class B_IdleState : BalloonState
{
    public B_IdleState(BalloonStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();
    }
}

public class B_MovingState : BalloonState
{
    private bool isRunnig;
    public B_MovingState(BalloonStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        isRunnig = Input.GetKey(KeyCode.LeftShift);

        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();
    }
}

public class B_JumpingState : BalloonState
{
    public B_JumpingState(BalloonStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();
    }
}

public class B_FallingState : BalloonState
{
    public B_FallingState(BalloonStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        CheckTransitions();
    }

    public override void FixedUpdate()
    {
        playerController.HandleMovement();
    }
}


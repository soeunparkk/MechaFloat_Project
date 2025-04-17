using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator animator;
    public PlayerStateMachine stateMachine;

    // �ִϸ��̼� �Ķ���� �̸����� ����� ����
    private const string PARAM_IS_MOVING = "IsMoving";
    private const string PARAM_SPEED = "Speed";
    private const string PARAM_IS_JUMPING = "IsJumping";
    private const string PARAM_IS_FALLING = "IsFalling";
    private const string PARAM_IS_PICKING = "IsPicking";

    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if (stateMachine.currentState != null)
        {
            ResetAllBoolParameters();

            switch (stateMachine.currentState)
            {
                case IdleState:
                    break;
                case MovingState:
                    animator.SetBool(PARAM_IS_MOVING, true);

                    float speed = stateMachine.PlayerController.CurrentMoveSpeed;
                    animator.SetFloat(PARAM_SPEED, speed);
                    break;
                case JumpingState:
                    animator.SetBool(PARAM_IS_JUMPING, true);
                    break;
                case FallingState:
                    animator.SetBool(PARAM_IS_FALLING, true);
                    break;
                case PickupState:
                    animator.SetBool(PARAM_IS_PICKING, true);
                    break;
            }
        }
    }

    // ��� bool �Ķ���͸� �ʱ�ȭ ���ִ� �Լ�
    private void ResetAllBoolParameters()
    {
        animator.SetBool(PARAM_IS_MOVING, false);
        animator.SetBool(PARAM_IS_JUMPING, false);
        animator.SetBool(PARAM_IS_FALLING, false);
        animator.SetBool(PARAM_IS_PICKING, false);
    }
}
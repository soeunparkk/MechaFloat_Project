using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator animator;
    public PlayerStateMachine stateMachine;
    public PlayerJump playerJump;

    // 애니메이션 파라미터 이름들을 상수로 정의
    private const string PARAM_IS_MOVING = "IsMoving";
    private const string PARAM_IS_RUNNING = "IsRunning";
    private const string PARAM_IS_JUMPING = "IsJumping";
    private const string PARAM_IS_GROUND = "IsGround";
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
                    animator.SetBool(PARAM_IS_GROUND, true);
                    break;
                case MovingState:
                    animator.SetBool(PARAM_IS_MOVING, true);
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        animator.SetBool(PARAM_IS_RUNNING, true);
                    }
                    break;
                case JumpingState:
                    animator.SetBool(PARAM_IS_JUMPING, true);
                    animator.SetBool(PARAM_IS_GROUND, false);
                    break;
                case FallingState:
                    animator.SetBool(PARAM_IS_FALLING, true);
                    if (playerJump.GetVerticalVelocity() == 0)
                    {
                        animator.SetBool(PARAM_IS_GROUND, true);
                    }
                    break;
                case PickupState:
                    animator.SetBool(PARAM_IS_PICKING, true);
                    break;
            }
        }
    }

    // 모든 bool 파라미터를 초기화 해주는 함수
    private void ResetAllBoolParameters()
    {
        animator.SetBool(PARAM_IS_MOVING, false);
        animator.SetBool(PARAM_IS_RUNNING, false); 
        animator.SetBool(PARAM_IS_JUMPING, false);
        animator.SetBool(PARAM_IS_GROUND, false);
        animator.SetBool(PARAM_IS_FALLING, false);
        animator.SetBool(PARAM_IS_PICKING, false);
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator animator;
    public PlayerStateMachine stateMachine;

    // �ִϸ��̼� �Ķ���� �̸����� ����� ����
    /*private const string PARAM_IS_RUNNING = "IsRunning";
    private const string PARAM_IS_DASHING = "IsDashing";*/

    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        /*if (stateMachine.currentState != null)
        {
            ResetAllBoolParameters();

            switch (stateMachine.currentState)
            {
                case IdleState:
                    break;
                case RunningState:
                    animator.SetBool(PARAM_IS_RUNNING, true);
                    break;
                case DashingState:
                    animator.SetBool(PARAM_IS_DASHING, true);
                    break;
            }
        }*/
    }

    // ��� bool �Ķ���͸� �ʱ�ȭ ���ִ� �Լ�
    private void ResetAllBoolParameters()
    {
        /*animator.SetBool(PARAM_IS_RUNNING, false);
        animator.SetBool(PARAM_IS_DASHING, false);*/
    }
}
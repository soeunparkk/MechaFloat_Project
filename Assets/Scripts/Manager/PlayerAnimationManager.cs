using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    public Animator animator;
    public PlayerStateMachine stateMachine;

    private PlayerController controller; 
    private PlayerJump playerJump;

    // �ִϸ��̼� �Ķ���� �̸����� ����� ����
    private const string PARAM_IS_MOVING = "IsMoving";  
    private const string PARAM_IS_RUNNING = "IsRunning";    
    private const string PARAM_IS_JUMPING = "IsJumping"; 
    private const string PARAM_IS_GROUND = "IsGround";
    private const string PARAM_IS_FALLING = "IsFalling";
    private const string PARAM_IS_PICKING = "IsPicking";

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        playerJump = GetComponent<PlayerJump>();
    }

    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if (stateMachine.currentState == null) return;

        ResetAllBoolParameters();

        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        animator.SetBool(PARAM_IS_MOVING, isMoving);
        animator.SetBool(PARAM_IS_RUNNING, isRunning);

        switch (stateMachine.currentState)
        {
            case IdleState:
                animator.SetBool(PARAM_IS_GROUND, true);
                SetBalloonAnimationState("Wait");
                break;
            case MovingState:
                animator.SetBool(PARAM_IS_GROUND, true);
                SetBalloonAnimationState(isRunning ? "Running" : "Walking");
                break;
            case JumpingState:
                animator.SetBool(PARAM_IS_JUMPING, true);
                animator.SetBool(PARAM_IS_GROUND, false);
                SetBalloonAnimationState("Jump");
                break;
            case FallingState:
                animator.SetBool(PARAM_IS_FALLING, true);
                if (playerJump.GetVerticalVelocity() == 0f)
                    animator.SetBool(PARAM_IS_GROUND, true);
                SetBalloonAnimationState("Jump"); // Ȥ�� "Fall"�� �ִٸ� Fall��
                break;
            case PickupState:
                animator.SetBool(PARAM_IS_PICKING, true);
                SetBalloonAnimationState("Wait");
                break;
        }
    }


    // ��� bool �Ķ���͸� �ʱ�ȭ ���ִ� �Լ�
    private void ResetAllBoolParameters()
    {
        animator.SetBool(PARAM_IS_MOVING, false);
        animator.SetBool(PARAM_IS_RUNNING, false); 
        animator.SetBool(PARAM_IS_JUMPING, false);
        animator.SetBool(PARAM_IS_GROUND, false);
        animator.SetBool(PARAM_IS_FALLING, false);
        animator.SetBool(PARAM_IS_PICKING, false);
    }

    private void SetBalloonAnimationState(string stateName)
    {
        // ���� ������ ǳ�� ��������
        BalloonController equippedBalloon = InventoryManager.Instance.GetEquippedBalloon();
        if (equippedBalloon == null) return;

        // ��� �ڽ� ��Ȱ��ȭ
        foreach (Transform child in equippedBalloon.transform)
        {
            child.gameObject.SetActive(child.name == stateName);
        }
    }
}
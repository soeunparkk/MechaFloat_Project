using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator balloonAnimator;

    public BalloonStateMachine stateMachine;

    private PlayerController controller;
    private PlayerJump playerJump;

    private const string PARAM_IS_B_MOVING = "IsB_Moving";
    private const string PARAM_IS_B_RUNNING = "IsB_Running";
    private const string PARAM_IS_B_JUMPING = "IsB_Jumping";
    private const string PARAM_IS_B_FALLING = "IsB_Falling";
    private const string PARAM_IS_B_GROUND = "IsGround";

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
        playerJump = FindObjectOfType<PlayerJump>();
    }

    void Update()
    {
        BallonUpdateAnimationState();
    }

    void BallonUpdateAnimationState()
    {
        if (stateMachine.currentState != null)
        {
            ResetAll();

            bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
            bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

            balloonAnimator.SetBool(PARAM_IS_B_MOVING, isMoving);
            balloonAnimator.SetBool(PARAM_IS_B_RUNNING, isRunning);

            switch (stateMachine.currentState)
            {
                case B_IdleState:
                    balloonAnimator.SetBool(PARAM_IS_B_GROUND, true);
                    break;

                case B_MovingState:
                    balloonAnimator.SetBool(PARAM_IS_B_GROUND, true);
                    break;

                case B_JumpingState:
                    balloonAnimator.SetBool(PARAM_IS_B_JUMPING, true);
                    balloonAnimator.SetBool(PARAM_IS_B_GROUND, false);
                    break;

                case B_FallingState:
                    balloonAnimator.SetBool(PARAM_IS_B_FALLING, true);
                    if (playerJump.GetVerticalVelocity() == 0)
                    {
                        balloonAnimator.SetBool(PARAM_IS_B_GROUND, true);
                    }
                    break;
            }
        }
    }

    private void ResetAll()
    {
        balloonAnimator.SetBool(PARAM_IS_B_MOVING, false);
        balloonAnimator.SetBool(PARAM_IS_B_RUNNING, false);
        balloonAnimator.SetBool(PARAM_IS_B_JUMPING, false);
        balloonAnimator.SetBool(PARAM_IS_B_FALLING, false);
        balloonAnimator.SetBool(PARAM_IS_B_GROUND, false);
    }
}
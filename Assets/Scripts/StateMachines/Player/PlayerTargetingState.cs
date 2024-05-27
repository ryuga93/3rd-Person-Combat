using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    readonly int TargetingBlendTreeHash = Animator.StringToHash("Targeting Blend Tree");
    readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");
    const float CrossFadeDuration = 0.4f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
        
        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
        UpdateAnimator(deltaTime);
        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    void OnTarget()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    void OnDodge()
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            return;
        }
        
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }

    void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    void UpdateAnimator(float deltaTime)
    {
        Vector2 movement = stateMachine.InputReader.MovementValue;
        stateMachine.Animator.SetFloat(TargetingForwardHash, movement.y == 0 ? 0f : Mathf.Sign(movement.y), 0.1f, deltaTime);
        stateMachine.Animator.SetFloat(TargetingRightHash, movement.x == 0 ? 0f : Mathf.Sign(movement.x), 0.1f, deltaTime);
    }
}

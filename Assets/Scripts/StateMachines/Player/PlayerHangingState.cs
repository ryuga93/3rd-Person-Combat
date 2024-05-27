using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    readonly int HangingHash = Animator.StringToHash("Hanging");
    const float CrossFadeDuration = 0.1f;

    Vector3 ledgeForward;
    Vector3 closestPoint;

    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine)
    {
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
    }

    public override void Enter()
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);

        stateMachine.CharacterController.enabled = false;
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position); // new body pos = position of hand - (distance between hand and body)
        stateMachine.CharacterController.enabled = true;
        
        stateMachine.Animator.CrossFadeInFixedTime(HangingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue.y > 0)
        {
            stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
        }
        else if (stateMachine.InputReader.MovementValue.y < 0)
        {
            stateMachine.CharacterController.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}

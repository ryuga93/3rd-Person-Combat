using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    readonly int BlockHash = Animator.StringToHash("Block");
    const float CrossFadeDuration = 0.1f;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (!stateMachine.InputReader.IsBlocking)
        {
        ReturnToLocomotion();
        return;
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }
}

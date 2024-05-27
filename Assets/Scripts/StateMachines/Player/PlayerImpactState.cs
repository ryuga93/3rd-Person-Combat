using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState

{
    readonly int ImpactHash = Animator.StringToHash("Impact");
    const float CrossFadeDuration = 0.1f;
    float impactDuration = 1f;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        impactDuration -= deltaTime;

        if (impactDuration <= 0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected bool IsInChaseRange()
    {
        if (stateMachine.Player.IsDead) { return false; }
        
        Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;
        float distance = toPlayer.sqrMagnitude;

        return distance <= stateMachine.PlayerChasingRange * stateMachine.PlayerChasingRange;
    }

    protected bool IsInAttackingRange()
    {
        if (stateMachine.Player.IsDead) { return false; }

        Vector3 toPlayer = stateMachine.Player.transform.position - stateMachine.transform.position;
        float distance = toPlayer.sqrMagnitude;

        return distance <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElemental : Monster
{
    public MonsterStateMachine<MONSTER_STATE, FireElemental> stateMachine;

    public FireElemental(string unitName) : base(unitName) { }

    public override void Awake()
    {
        base.Awake();

        stateMachine = new MonsterStateMachine<MONSTER_STATE, FireElemental>();
        stateMachine.Init(this);

        stateMachine.AddState(MONSTER_STATE.IDLE, new FireElementalState.IdleState());
        stateMachine.AddState(MONSTER_STATE.TRACE, new FireElementalState.TraceState());
        stateMachine.AddState(MONSTER_STATE.ATTACK, new FireElementalState.AttackState());
        stateMachine.AddState(MONSTER_STATE.HIT, new FireElementalState.HitState());
        stateMachine.AddState(MONSTER_STATE.DIE, new FireElementalState.DieState());

        stateMachine.ChangeState(MONSTER_STATE.IDLE);
    }

    private void Update()
    {
        Gravity();
        stateMachine.Update();
    }

    public void ChangeState(MONSTER_STATE nextState)
    {
        stateMachine.ChangeState(nextState);
    }

    public override void TakeHit(float damage)
    {
        if (CurHp <= 0) return;

        base.TakeHit(damage);
        stateMachine.ChangeState(MONSTER_STATE.HIT);
    }

    public override void Die()
    {
        playerController.PlayerXP += XP;
        stateMachine.ChangeState(MONSTER_STATE.DIE);
    }
}

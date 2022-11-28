using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raptor : Monster
{
    public MonsterStateMachine<MONSTER_STATE, Raptor> stateMachine;
    public Raptor(string unitName) : base(unitName) { }

    public override void Awake()
    {
        base.Awake();
        stateMachine = new MonsterStateMachine<MONSTER_STATE, Raptor>();
        stateMachine.Init(this);
        stateMachine.AddState(MONSTER_STATE.IDLE, new RaptorState.IdleState());
        stateMachine.AddState(MONSTER_STATE.TRACE, new RaptorState.TraceState());
        stateMachine.AddState(MONSTER_STATE.ATTACK, new RaptorState.AttackState());
        stateMachine.AddState(MONSTER_STATE.HIT, new RaptorState.HitState());
        stateMachine.AddState(MONSTER_STATE.DIE, new RaptorState.DieState());

        stateMachine.ChangeState(MONSTER_STATE.IDLE);
    }

    private void Update()
    {
        Gravity();
        stateMachine.Update();
        Debug.Log(gameObject.transform.localRotation);
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

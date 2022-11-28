using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

namespace FireElementalState
{
    public class BaseState : State<FireElemental>
    {
        public override void Enter(FireElemental Owner)
        {

        }
        public override void Update(FireElemental Owner)
        {
        }
        public override void Exit(FireElemental Owner)
        {
        }
    }

    public class IdleState : BaseState
    {
        float timer = 0;
        [SerializeField] public int randNum;
        public override void Enter(FireElemental Owner)
        {
            Owner.animator.SetBool("Trace", false);
        }

        public override void Update(FireElemental Owner)
        {
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                timer = 0;
            }

            if (timer == 0)
            {
                randNum = Random.Range(0, 100);

                if (randNum >= 95)
                    Owner.animator.SetTrigger("StandChangeVer2");
                else if (randNum >= 80)
                    Owner.animator.SetTrigger("StandChangeVer1");
            }

            Owner.ViewDetector.FindTarget();
            GameObject target = Owner.ViewDetector.target;

            if (target != null)
                Owner.ChangeState(MONSTER_STATE.TRACE);
        }

        public override void Exit(FireElemental Owner)
        {
        }
    }
    public class TraceState : BaseState
    {
        public override void Enter(FireElemental Owner)
        {
            Owner.animator.SetBool("Trace", true);
        }

        public override void Update(FireElemental Owner)
        {
            Owner.ViewDetector.FindTarget();
            GameObject traceTarget = Owner.ViewDetector.target;

            if (traceTarget == null)
            {
                Owner.ChangeState(MONSTER_STATE.IDLE);
                return;
            }
            Vector3 targetLookAt = new Vector3(traceTarget.transform.position.x, Owner.transform.position.y, traceTarget.transform.position.z);

            Vector3 moveDir = traceTarget.transform.position - Owner.transform.position;
            Owner.characterController.Move(moveDir.normalized * Time.deltaTime * Owner.MoveSpeed);
            Owner.animator.SetFloat("moveSpeed", Owner.MoveSpeed);
            Owner.transform.LookAt(targetLookAt);

            GameObject attackTarget;
            Collider[] targets = Physics.OverlapSphere(Owner.transform.position, Owner.AttackRange, Owner.TargetLayerMask);
            if (targets.Length > 0 && Owner.AttackRange >= moveDir.magnitude)
            {
                attackTarget = targets[0].gameObject;
                Owner.ChangeState(MONSTER_STATE.ATTACK);
                return;
            }
            else
            {
                attackTarget = null;
            }
        }

        public override void Exit(FireElemental Onwer)
        {

        }
    }
    public class AttackState : BaseState
    {
        float timer = 0;
        public override void Enter(FireElemental Owner)
        {
            Owner.animator.SetBool("Trace", !Owner.animator.GetBool("Trace"));
        }

        public override void Update(FireElemental Owner)
        {
            timer += Time.deltaTime;
            if (timer >= Owner.attackDelayTime) timer = 0;

            if (Owner.ViewDetector.target == null)
            {
                Owner.ChangeState(MONSTER_STATE.TRACE);
                return;
            }

            Vector3 moveDir = Owner.ViewDetector.target.transform.position - Owner.transform.position;
            if (Owner.AttackRange <= moveDir.magnitude)
            {
                Owner.ChangeState(MONSTER_STATE.TRACE);
                return;
            }

            Owner.animator.SetBool("Attack", false);
            if (timer == 0)
            {
                Owner.animator.SetBool("Attack", true);
                Owner.ViewDetector.target.GetComponent<IDamagable>().TakeHit(Owner.Atk);
            }
        }

        public override void Exit(FireElemental Owner)
        {
            Owner.animator.SetBool("Attack", false);
        }
    }
    public class HitState : BaseState
    {
        public override void Enter(FireElemental Owner)
        {
            Owner.animator.SetTrigger("Hit");
        }

        public override void Update(FireElemental Owner)
        {
            if (Owner.CurHp <= 0) Owner.ChangeState(MONSTER_STATE.DIE);
        }

        public override void Exit(FireElemental Owner)
        {
        }
    }
    public class DieState : BaseState
    {
        IEnumerator dieTimer;
        public override void Enter(FireElemental Owner)
        {
            Owner.animator.SetTrigger("Die");
            dieTimer = Owner.RootTimer();
            Owner.StartCoroutine(dieTimer);
        }

        public override void Update(FireElemental Owner)
        {
            if (Owner.onRoot)
            {
                Owner.targetManager.targetInfo.SetActive(false);
                MonsterSpawner.Instance.InsertQueue(Owner.gameObject);
                Owner.ChangeState(MONSTER_STATE.IDLE);
            }
        }

        public override void Exit(FireElemental Owner)
        {
            Owner.onRoot = false;
            Owner.StopCoroutine(dieTimer);
            Owner.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine<T1, T2> where T2 : MonoBehaviour
{
    private T2 Owner;
    public State<T2> curState;
    private Dictionary<T1, State<T2>> states;

    public void Init(T2 Owner)
    {
        this.Owner = Owner;
        curState = null;
        states = new Dictionary<T1, State<T2>>();
    }

    public void Update()
    {
        curState.Update(Owner);
    }

    public void AddState(T1 type, State<T2> state)
    {
        states.Add(type, state);
    }

    public void ChangeState(T1 type)
    {
        if (curState != null)
            curState.Exit(Owner);
        curState = states[type];
        curState.Enter(Owner);
    }
}
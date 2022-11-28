using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : Unit, IDamagable
{
    [Header("Monster Stats")]
    public TargetManager targetManager;
    public PlayerController playerController;

    [SerializeField] private int xp;
    public int XP => xp;

    public EnemyUnit(string unitName) : base(unitName)
    {
    }

    public virtual void Start()
    {
        targetManager = GameObject.FindObjectOfType<TargetManager>().GetComponent<TargetManager>();
        playerController = GameObject.FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
    }


    public virtual void TakeHit(float damage)
    {
        CurHp -= damage;
        targetManager.UpdateTargetInfo(targetManager.target);
    }

    public virtual void Attack()
    { }


}

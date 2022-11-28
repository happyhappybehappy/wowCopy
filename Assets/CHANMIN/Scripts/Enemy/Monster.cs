using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MONSTER_STATE
{
    IDLE,
    TRACE,
    ATTACK,
    HIT,
    DIE,
}

public class Monster : EnemyUnit, IIsFlyed
{
    [SerializeField] private int atk;
    public int Atk => atk;
    [SerializeField] private float attackRange;
    public float AttackRange => attackRange;

    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;

    public float attackDelayTime;


    [SerializeField] private bool isFly = false;
    public bool IsFly { get => isFly; set => isFly = value; }

    private ViewDetector viewDetector;
    public ViewDetector ViewDetector => viewDetector;

    [Space(10f)]
    [Header("ETC")]
    /***
     * 드랍템 습득 여부로 true/false 정하기
     ***/
    public bool onRoot;

    public Image image;

    protected MONSTER_STATE state;
    public Animator animator;

    [SerializeField] private LayerMask targetLayerMask;
    public LayerMask TargetLayerMask => targetLayerMask;


    public CharacterController characterController;

    [Space(10f)]
    [Header("Target")]

    public GameObject traceTarget;
    public GameObject attackTarget;


    public Monster(string unitName) : base(unitName) { }

    public virtual void Awake()
    {
        CurHp = Hp;
        viewDetector = GetComponent<ViewDetector>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    public void OnEnable()
    {
        CurHp = Hp;
        viewDetector = GetComponent<ViewDetector>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }


    public virtual void OnTriggerStay(Collider other)
    {
    }

    public IEnumerator RootTimer()
    {
        yield return new WaitForSeconds(10f);
        if (onRoot == false)
        {
            onRoot = true;
        }
    }


    public void SetFalseActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Gravity()
    {
        if (characterController.isGrounded) 
            return;

        else
            characterController.Move(Vector3.up * -5f * Time.deltaTime);
    }
}

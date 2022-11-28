using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using System.Runtime.InteropServices;
using UniRx;

using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Unit, IDamagable
{
    private CharacterController controller;
    public UIManager uiManager;
    public XPManger xpManager;
    private float moveY;

    #region ======================================Player Stats======================================
    [Header("[Player Stats]")]


    [SerializeField] private float damage;
    public float Damage => damage;

    [SerializeField] private int furyGage;
    public int FuryGage
    {
        get { return furyGage; }
        set
        {
            furyGage = value;
            StartCoroutine(uiManager.UpdatePlayerFuryCo());

            if (furyGage >= 100) furyGage = 100;
        }
    }

    [SerializeField] private int level;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            uiManager.UpdatePlayerLevelUp();
        }
    }

    [SerializeField] private float playerXP;
    public float PlayerXP
    {
        get { return playerXP; }
        set
        {
            playerXP = value;
            xpManager.CompareXP(ref playerXP);
            StartCoroutine(uiManager.UpdateXPCo());
        }
    }

    #endregion

    #region ======================================Move Stats======================================
    [Space(10f)]
    [Header("[Move Stats]")]

    [SerializeField] private float weight = 2f;

    [SerializeField] private float moveSpeed = 4f;
    public float MoveSpeed => moveSpeed;

    [SerializeField] private float walkRate = 0.2f;
    public float WalkRate => walkRate;

    public float jumpTime;
    [SerializeField] private float jumpSpeed = 5f;
    public float JumpSpeed => jumpSpeed;

    public bool isJump;
    public bool isFall;

    private bool isGrounded;
    public bool IsGrounded => isGrounded;
    public GroundChecker groundChecker;

    [Space(10f)]


    #endregion

    #region ======================================ETC======================================
    [Header("[ETC]")]

    public Animator animator;
    public Transform enemy;
    [SerializeField] private bool playerAttack;
    public bool PlayerAttack
    {
        get { return playerAttack; }
        set { playerAttack = value; }
    }

    [SerializeField] private bool playerSkillAttack;
    public bool PlayerSkillAttack => playerSkillAttack;

    public Transform interactionPoint;
    public float interactionRange = 1f;
    [SerializeField] private LayerMask hitLayerMask;
    [SerializeField] private GameObject mount;
    [SerializeField] private GameObject mountPlayer;
    [SerializeField] private GameObject mountCameraPoint;
    [SerializeField] private CinemachineFreeLook cineCam;

    #endregion


    public PlayerController(string unitName) : base(unitName) { }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        groundChecker = GetComponentInChildren<GroundChecker>().GetComponent<GroundChecker>();
        animator = GetComponent<Animator>();

        CurHp = Hp;
        Level = 1;
    }

    private void Start()
    {
        controller.ObserveEveryValueChanged(x => x.isGrounded).ThrottleFrame(5).Subscribe(x => isGrounded = x);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            playerModCchange();
        }
        if (FindObjectOfType<HeroicLeap>().GetComponent<HeroicLeap>().isHeroicLeap == false && isFall == false)
            Gravity();
        if (FindObjectOfType<HeroicLeap>().GetComponent<HeroicLeap>().isHeroicLeap == false && isJump == false)
            FallGravity();
    }

    private void Gravity()
    {
        if (jumpTime >= 0f)
        {
            isJump = true;
            moveY = jumpSpeed;
            jumpTime -= Time.deltaTime;
        }
        else if (controller.isGrounded)
        {
            animator.SetFloat("JumpHeight", 0);
            isJump = false;
            moveY = 0;
            return;
        }
        else
            moveY += Physics.gravity.y * weight * Time.deltaTime;

        controller.Move(Vector3.up * moveY * Time.deltaTime);
        animator.SetFloat("JumpHeight", 10.2f);
    }

    private void FallGravity()
    {
        if (controller.isGrounded)
        {
            animator.SetBool("Fall", false);
            isFall = false;
            return;
        }

        else if (groundChecker.IsFall)
        {
            controller.Move(Vector3.up * -jumpSpeed * Time.deltaTime);
            animator.SetBool("Fall", false);
            isFall = false;
            return;
        }
        else
        {
            animator.SetBool("Fall", true);

            controller.Move(Vector3.up * -jumpSpeed  * Time.deltaTime);
        }
    }
    public void TakeHit(float damage)
    {
        CurHp -= damage;
        StartCoroutine(uiManager.UpdatePlayerHPCo());
        animator.SetTrigger("Hit");
        PlayerAttack = true;
        if (CurHp <= 0) Die();
    }
    public override void Die()
    {
        animator.SetTrigger("Die");
    }

    public void playerModCchange()
    {
        mount.SetActive(true);
        mount.transform.position = gameObject.transform.position;
        cineCam.Follow = mount.transform;
        cineCam.LookAt = mountCameraPoint.transform;
        transform.gameObject.SetActive(false);
    }
}

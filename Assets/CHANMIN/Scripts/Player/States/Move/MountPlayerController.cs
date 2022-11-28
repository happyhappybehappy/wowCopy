using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using System.Runtime.InteropServices;

[RequireComponent(typeof(CharacterController))]
public class MountPlayerController : Unit, IDamagable
{
    public MountPlayerController(string unitName) : base(unitName) { }

    private CharacterController characterController;
    private float moveY;
    [SerializeField] private int furyGage;
    public int FuryGage
    {
        get { return furyGage; }
        set { furyGage = value; }
    }

    [Header("[Move Stats]")]

    [SerializeField] private float moveSpeed = 12f;
    public float MoveSpeed => moveSpeed;

    [SerializeField] private float walkRate = 0.2f;
    public float WalkRate => walkRate;

    public float jumpTime;
    [SerializeField] private float jumpSpeed = 5f;
    public float JumpSpeed => jumpSpeed;

    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    [Space(10f)]

    public GroundChecker groundChecker;
    public Animator animator;

    [SerializeField] private GameObject myPlayer;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCameraPoint;
    [SerializeField] private CinemachineFreeLook cineCam;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        groundChecker = GetComponentInChildren<GroundChecker>().GetComponent<GroundChecker>();
        animator = GetComponent<Animator>();
        CurHp = Hp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            playerModCchange();
    }

    public void TakeHit(float damage)
    {
        CurHp -= damage;
        playerModCchange();
    }

    public void playerModCchange()
    {
        player.SetActive(true);
        player.GetComponent<PlayerController>().CurHp = CurHp;
        player.GetComponent<PlayerController>().FuryGage = FuryGage;
        player.transform.position = gameObject.transform.position;
        cineCam.Follow = player.gameObject.transform;
        cineCam.LookAt = playerCameraPoint.transform;
        gameObject.SetActive(false);
    }
}

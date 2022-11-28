using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PlayerFindTarget : MonoBehaviour
{
    public Transform player;
    PlayerController playerController;


    public Collider[] targets = new Collider[3];

    public LayerMask enermyTarget;
    public Animator animator;
    public GameObject targetInfomation;
    [SerializeField] private TargetManager targetManager;

    private int curIndex = 0;

    void Start()
    {
        player = GetComponent<Transform>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        FindInteractiveTarget();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TargetSort();
            if (targets.Length >= 3)
                curIndex = ++curIndex % 3;
            else if (targets.Length == 2)
                curIndex = ++curIndex % targets.Length;
            else curIndex = 0;
        }
    }

    private void FindInteractiveTarget()
    {
        targets = Physics.OverlapSphere(playerController.interactionPoint.position, playerController.interactionRange, enermyTarget);
        targets = targets.OrderBy(x => Vector3.Distance(player.transform.position, x.transform.position)).ToArray();
    }
    private void TargetSort()
    {
        if (targets.Length == 0)
        {
            targetInfomation.SetActive(false);
            playerController.PlayerAttack = false;
            return;
        }
        targetInfomation.SetActive(true);
        targetManager.target = targets[curIndex].transform.gameObject;

        targetManager.UpdateTargetInfo(targetManager.target);

        playerController.enemy = targetManager.target.transform;
        targetManager.targetInteractablePos = targetManager.target.transform.position;
    }

}

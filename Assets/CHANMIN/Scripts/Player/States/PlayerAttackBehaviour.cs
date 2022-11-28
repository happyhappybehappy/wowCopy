using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : StateMachineBehaviour
{
    private Animator animator;
    private PlayerController playerController = null;
    [SerializeField] private TargetManager targetManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;

        playerController = animator.GetComponent<PlayerController>();

        playerController.FuryGage += 5;
        targetManager = FindObjectOfType<TargetManager>().GetComponent<TargetManager>();

        targetManager.target.GetComponent<IDamagable>()?.TakeHit(playerController.Damage);
        
    }

}

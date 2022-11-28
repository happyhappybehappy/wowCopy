using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyableMountMoveBehaviour : MoveBehaviour
{
    public bool isFly;
    public float flySpeed;
    [SerializeField] private MountPlayerController mountPlayerController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (mountPlayerController == null)
            mountPlayerController = animator.GetComponent<MountPlayerController>();

        isFly = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
        JumpFly();
    }

    public override void Move()
    {
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) return;

        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (moveInput.sqrMagnitude > 1f)
        {
            moveInput = moveInput.normalized;
        }

        if (Input.GetButton("Walk"))
        {
            curRate -= Time.deltaTime;
            if (curRate < mountPlayerController.WalkRate)
                curRate = mountPlayerController.WalkRate;
        }
        else
        {
            curRate += Time.deltaTime;
            if (curRate > 1f)
                curRate = 1f;
        }
        moveInput *= curRate;

        Vector3 forwardVec = new Vector3(camera.transform.forward.x, 0f, camera.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(camera.transform.right.x, 0f, camera.transform.right.z).normalized;

        Vector3 moveVec = moveInput.x * rightVec + moveInput.z * forwardVec;

        if (moveVec.magnitude > 0f)
        {
            animator.transform.forward = moveVec;
        }
        animator.SetFloat("MoveSpeed", moveInput.magnitude);

        characterController.Move(moveVec * mountPlayerController.MoveSpeed * Time.deltaTime);
    }

    public void JumpFly()
    {
        if (!isFly && Input.GetButton("JumpFly"))
        {
            animator.SetTrigger("JumpFly");
            isFly = true;
        }

        else if (isFly && Input.GetButton("JumpFly"))
        {
            characterController.Move(Vector3.up * flySpeed * Time.deltaTime);
        }
    }
}

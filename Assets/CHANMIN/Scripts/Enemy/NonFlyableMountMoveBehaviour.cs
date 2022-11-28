using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.TestTools;

public class NonFlyableMountMoveBehaviour : MoveBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
        Jump();
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
            if (curRate < playerController.WalkRate)
                curRate = playerController.WalkRate;
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

        characterController.Move(moveVec * playerController.MoveSpeed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpBehaviour : StateMachineBehaviour
{
    private Animator animator;
    private CharacterController characterController = null;
    private PlayerController playerController = null;
    private Camera camera;

    private float curRate;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;

        if (characterController == null)   characterController = animator.GetComponent<CharacterController>();
        if (playerController == null)
        {
            playerController = animator.GetComponent<PlayerController>();
            camera = Camera.main;
        }

        Jump();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
    }

    private void Move()
    {
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
        // 이동 방향 바라보기
        if (moveVec.magnitude > 0f)
            animator.transform.forward = moveVec;
        animator.SetFloat("MoveSpeed", moveInput.magnitude);

        characterController.Move(moveVec * playerController.MoveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        playerController.jumpTime = 0.2f;

    }
}

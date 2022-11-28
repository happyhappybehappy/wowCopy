using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : StateMachineBehaviour
{
    public Animator animator;
    public Transform transform;
    public CharacterController characterController;
    public PlayerController playerController;
    public Camera camera;
    public float curRate;

    public Vector3 moveInput;
    public Vector3 rotateInput;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        this.transform = animator.transform;

        if (characterController == null)
            characterController = animator.GetComponent<CharacterController>();

        if (playerController == null)
            playerController = animator.GetComponent<PlayerController>();


        camera = Camera.main;
    }

    public virtual void Move()
    {
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) return;
        moveInput = new Vector3(0f, 0f, Input.GetAxis("Vertical"));
        rotateInput = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

        if (moveInput.sqrMagnitude > 1f)
            moveInput = moveInput.normalized;

        if (rotateInput != Vector3.zero && moveInput == Vector3.zero)
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * 1f, 0f));
            animator.SetFloat("HoriSpeed", rotateInput.magnitude);
        }

        animator.SetFloat("HoriSpeed", rotateInput.magnitude);

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
            animator.transform.forward = moveVec;

        animator.SetFloat("MoveSpeed", moveInput.magnitude);
        characterController.Move(playerController.MoveSpeed * Time.deltaTime * moveVec);
    }

    public virtual void Jump()
    {
        if (playerController.isJump) return;

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Jump");
        }
    }
}

using System;
using System.Collections;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;

public class TargetManager : Clickable
{
    [SerializeField] Texture2D attackIcon;
    [SerializeField] Texture2D handIcon;
    [SerializeField] GameObject player;
    [SerializeField] GameObject mount;
    [SerializeField] bool isClicked;

    public UIManager uIManager;

    CharacterController characterController;
    [SerializeField] PlayerController playerController;

    enum CursorType
    {
        None,
        Hand,
        Attack,
    }
    CursorType cursorType = CursorType.None;

    public GameObject targetInfo;
    public GameObject targetSkillPos;

    public ParticleManager particleManager;
    private bool isMouseOver = false;
    public bool isSkillParticle;
    Vector3 dir;
    Animator anim;
    IEnumerator moveClick;

    private void Start()
    {
        Cursor.visible = true;
        if (particleManager == null)
            particleManager = GetComponent<ParticleManager>();

        if (player.activeSelf == true)
            characterController = player.GetComponent<CharacterController>();
        else if (mount.activeSelf == true)
            characterController = mount.GetComponent<CharacterController>();

        playerController = player.GetComponent<PlayerController>();


        targetUIOff();
        moveClick = null;

        anim = player.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            playerController.PlayerXP += 3;

        OnMouseOver();

        if (Input.GetMouseButtonUp(0))
            OnTargetLeft();

        else if (Input.GetMouseButtonUp(1))
        {
            OnTargetRight();
            if (playerController.PlayerAttack == true && target.GetComponent<IDamagable>() != null)
            {
                float randNum = UnityEngine.Random.Range(0, 100);

                player.transform.LookAt(new Vector3(target.transform.position.x, player.transform.position.y, target.transform.position.z));
                if (randNum >= 50)
                    playerController.animator.SetTrigger("AttackVer1");
                else
                    playerController.animator.SetTrigger("AttackVer2");

                playerController.PlayerAttack = false;
                StopCoroutine(moveClick);
            }

            else if (moveClick == null)
            {
                moveClick = MouseClickMove();
                StartCoroutine(moveClick);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            targetUIOff();
            if (player.GetComponent<PlayerController>().animator.GetFloat("MoveSpeed") != 0)
            {
                StopCoroutine(moveClick);
                moveClick = null;
                player.GetComponent<PlayerController>().animator.SetFloat("MoveSpeed", 0);
            }
        }
    }

    IEnumerator MouseClickMove()
    {
        dir = targetPos - player.transform.position;
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);
        Vector3 targetMovePos = new Vector3(targetPos.x, player.transform.position.y, targetPos.z);


        if (target.GetComponent<IIsFlyed>() != null && target.GetComponent<IIsFlyed>().IsFly == true)
            targetPos = new Vector3(target.transform.position.x, player.transform.position.y, target.transform.position.z);


        while (dir.magnitude > 0.1f)
        {
            targetMovePos = new Vector3(targetPos.x, player.transform.position.y, targetPos.z);
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || Input.GetButton("Jump")) 
            {
                moveClick = null;
                break;
            }


            dir = targetMovePos - player.transform.position;
            dirXZ = new Vector3(dir.x, 0f, dir.z);

            player.transform.rotation = Quaternion.LookRotation(dirXZ);
            anim.SetFloat("MoveSpeed", 1f);
            characterController.Move(dir.normalized * Time.deltaTime * playerController.MoveSpeed);
            yield return null;

        }
        anim.SetFloat("MoveSpeed", 0f);
        moveClick = null;
    }
    public void OnTargetRaycast(string layerMask, Action<RaycastHit> ifAction, Action<RaycastHit> elseAction = null)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f))
        {
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer(layerMask))
            {
                ifAction(raycastHit);
            }

            else
            {
                if (elseAction == null)
                {
                    return;
                }
                else elseAction(raycastHit);
            }
        }
    }
    public void OnTargetRaycast(string layerMask, string layerMask2, Action<RaycastHit> ifAction, Action<RaycastHit> elseAction = null)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f))
        {
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer(layerMask) || raycastHit.collider.gameObject.layer == LayerMask.NameToLayer(layerMask2))
            {
                ifAction(raycastHit);
            }

            else
            {
                if (elseAction == null)
                {
                    return;
                }
                else elseAction(raycastHit);
            }
        }
    }
    public void OnTargetRaycast(string layerMask, string layerMask2, string layerMask3, Action<RaycastHit> ifAction, Action<RaycastHit> elseAction = null)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f))
        {
            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer(layerMask) || raycastHit.collider.gameObject.layer == LayerMask.NameToLayer(layerMask2) || raycastHit.collider.gameObject.layer == LayerMask.NameToLayer(layerMask3))
            {
                ifAction(raycastHit);
            }

            else
            {
                if (elseAction == null)
                {
                    return;
                }
                else elseAction(raycastHit);
            }
        }
    }

    public override void OnTargetRight() // 이동, 공격
    {
        OnTargetRaycast("Monster", "Unit", (RaycastHit raycastHit) =>
        {
            target = raycastHit.transform.gameObject;

            UpdateTargetInfo(target);
            playerController.PlayerAttack = true;
            targetUIOn();

            targetInteractablePos = raycastHit.point;
            targetPos = raycastHit.point;
            particleManager.ParticleControl(target, target.transform.localScale.magnitude + 1);

            if (Vector3.Distance(player.transform.position, new Vector3(target.transform.position.x, player.transform.position.y, target.transform.position.z)) < 3f)
                playerController.PlayerAttack = true;

            else
                playerController.PlayerAttack = false;
        });

        OnTargetRaycast("Ground", (RaycastHit raycastHit) =>
        {
            target = raycastHit.transform.gameObject;
            targetPos = raycastHit.point;
            particleManager.ParticleControl(targetPos, raycastHit);
        });
    }

    public override void OnTargetLeft() // 선택
    {
        if (isSkillParticle)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                FindObjectOfType<Skill>().GetComponent<ISKillActonTarget>().UseActionSkill(hit);
                isSkillParticle = false;
                targetSkillPos.SetActive(false);
            }
        }

        else
        {
            OnTargetRaycast("Monster", "Unit", (RaycastHit raycastHit) =>
            {
                target = raycastHit.transform.gameObject;

                UpdateTargetInfo(target);

                targetInfo.SetActive(true);
                targetInteractablePos = raycastHit.point;
                playerController.enemy = target.transform;
                particleManager.ParticleControl(target, target.transform.localScale.magnitude + 1);
            });
        }
    }

    public override void OnMouseOver()
    {
        if (isSkillParticle == false)
        {
            OnTargetRaycast("Monster",

         (RaycastHit raycastHit) =>
         {
             if (cursorType != CursorType.Attack)
             {
                 Cursor.SetCursor(attackIcon, new Vector2(attackIcon.width / 5, 0), CursorMode.Auto);
                 cursorType = CursorType.Attack;
             }
         },
         (RaycastHit raycastHit) =>
         {
             if (cursorType != CursorType.Hand)
             {
                 Cursor.SetCursor(handIcon, new Vector2(handIcon.width / 3, 0), CursorMode.Auto);
                 cursorType = CursorType.Hand;
             }
         });
        }

        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                isSkillParticle = false;
                targetSkillPos.SetActive(false);
            }
            else if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                targetSkillPos.transform.position = hit.point + new Vector3(0, 0.2f, 0);
                targetSkillPos.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
        }
    }

    public void UpdateTargetInfo(GameObject target)
    {
        uIManager.TargetInfoUpdate(target.GetComponent<Unit>().Portraits, target.GetComponent<Unit>().UnitName
                                   , target.GetComponent<Unit>().CurHp.ToString(), target.GetComponent<Unit>().Hp.ToString());
        StartCoroutine(uIManager.UpdateTargetHPCo());
    }

    public void targetUIOn()
    {
        targetInfo.SetActive(true);
        playerController.PlayerAttack = true;
    }

    public void targetUIOff()
    {
        targetInfo.SetActive(false);
        playerController.PlayerAttack = false;
    }


    public IEnumerator playerAttackWaitTimeCo()
    {
        playerController.PlayerAttack = false;
        yield return new WaitForSeconds(2f);
        playerController.PlayerAttack = true;
    }
}
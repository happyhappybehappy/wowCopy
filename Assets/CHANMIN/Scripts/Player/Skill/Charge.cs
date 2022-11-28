using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charge : Skill
{
    public Transform chargeTarget;
    Vector3 dir;
    public override void Awake()
    {
        base.Awake();
        skillName = "Charge";
        useSkill = true;
        coolDown = 6f;
        skillDamage = 10f;
        getFury = 20;
    }

    private void Update()
    {
        if (useSkill == true)
            chargeTarget = playerController.enemy;

        if (useSkill == true && playerController.enemy != null && Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerController.FuryGage += getFury;
            StartCoroutine(uimanager.UpdatePlayerFuryCo());

            StartCoroutine(CheckCoolDown(coolDown));
            chargeTarget = playerController.enemy;
            playerController.transform.LookAt(chargeTarget.position);
            StartCoroutine(OnSkill());
        }
    }


    public override IEnumerator OnSkill()
    {
        playerController.PlayerAttack = false;
        dir = new Vector3(chargeTarget.position.x - playerController.transform.position.x, playerController.transform.position.y, chargeTarget.position.z - playerController.transform.position.z);

        while (dir.magnitude > 5f)
        {
            if (chargeTarget == null) break;

            Vector3 targetMovePos = new Vector3(chargeTarget.position.x, playerController.transform.position.y, chargeTarget.position.z);
            dir = targetMovePos - playerController.transform.position;

            playerController.transform.LookAt(dir);
            playerController.animator.SetFloat("Charge", 1f);
            characterController.Move(dir.normalized * Time.deltaTime * 30f);

            yield return new WaitForEndOfFrame();
        }
        chargeTarget.GetComponent<IDamagable>().TakeHit(20f);
        playerController.animator.SetFloat("Charge", 0f);
        playerController.animator.SetTrigger("ChargeAttack");
        playerController.PlayerAttack = true;
    }
}

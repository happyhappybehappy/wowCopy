using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeStorm : Skill
{
    public GameObject bladeStormParticle;

    public override void Awake()
    {
        base.Awake();
        skillName = "BladeStorm";
        useSkill = true;
        coolDown = 2f;
        skillDamage = 10f;
        requirementFury = 20;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && playerController.FuryGage < requirementFury)
        {
            Debug.Log("분노가 부족합니다");
        }

        else if (useSkill == true && Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerController.FuryGage -= requirementFury;
            Instantiate(bladeStormParticle, player.transform);

            playerController.animator.SetTrigger("BladeStorm");
            StartCoroutine(CheckCoolDown(coolDown));
        }
    }
}

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
            Debug.Log("�г밡 �����մϴ�");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlWind : Skill
{
    private bool onWhirlWind;
    public GameObject whirlWindParticle;
    public GameObject temp;

    public override void Awake()
    {
        base.Awake();
        onWhirlWind = false;
        skillName = "WhirlWind";
        useSkill = true;
        coolDown = 12f;
        skillDamage = 4f;
        requirementFury = 80;
    }

    void Update()
    {

        if ((useSkill == false && onWhirlWind == true && Input.GetKeyDown(KeyCode.Alpha4)))
        {
            StopWhirWind(); 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && playerController.FuryGage < requirementFury)
        {
            Debug.Log("분노가 부족합니다");
        }

        else if (useSkill == true && Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerController.PlayerAttack = false;
            temp = Instantiate(whirlWindParticle, player.transform);

            StartCoroutine(CheckCoolDown(coolDown));
            StartCoroutine(StopWhirWindCo());
            playerController.FuryGage -= requirementFury;
            StartCoroutine(uimanager.UpdatePlayerFuryCo());

            playerController.animator.SetBool("WhirlWind", true);
            onWhirlWind = true;
        }

    }

    public IEnumerator StopWhirWindCo()
    {
        yield return new WaitForSeconds(coolDown-1);
        StopWhirWind();
    }

    public void StopWhirWind()
    {
        playerController.animator.SetBool("WhirlWind", false);
        onWhirlWind = false;
        playerController.PlayerAttack = true;
        Destroy(temp);
    }
}

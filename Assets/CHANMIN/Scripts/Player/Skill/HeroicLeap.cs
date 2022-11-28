using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroicLeap : Skill, ISKillActonTarget
{
    private Transform heroicLeapTarget;
    private Vector3 startPos, endPos;

    public ParticleSystem crackParticle;
    public bool isHeroicLeap = false;
    private float timer;

    public override void Awake()
    {
        base.Awake();

        skillName = "HeroicLeap";
        useSkill = true;
        coolDown = 10f;
        skillDamage = 5f;
        getFury = 20;
    }

    private void Update()
    {
        if (useSkill == true && Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (targetManager.isSkillParticle == false)
            {
                targetManager.targetSkillPos.SetActive(true);
                targetManager.isSkillParticle = true;
            }
        }
    }
    public void UseActionSkill(RaycastHit hit)
    {
        playerController.FuryGage += getFury;
        StartCoroutine(uimanager.UpdatePlayerFuryCo());

        heroicLeapTarget = hit.transform;
        playerController.animator.SetTrigger("HeroicLeap");
        startPos = playerController.transform.position;
        endPos = new Vector3(hit.point.x, hit.point.y + 0.2f, hit.point.z);
        StartCoroutine(CheckCoolDown(coolDown));
        StartCoroutine(OnSkill(hit));
    }

    public override IEnumerator OnSkill(RaycastHit hit)
    {
        playerController.PlayerAttack = false;
        timer = 0;
        float offsetTime = 0.5f;

        isHeroicLeap = true;
        while (playerController.transform.position.y >= startPos.y)
        {
            Vector3 targetMovePos = new Vector3(heroicLeapTarget.position.x, playerController.transform.position.y, heroicLeapTarget.position.z);


            Vector3 dir = targetMovePos - playerController.transform.position;
            playerController.transform.LookAt(dir);
            timer += Time.deltaTime;
            Vector3 tempPos = Parabola(startPos, endPos, 10, timer);
            playerController.transform.position = tempPos;

            if(timer > offsetTime)
            {
                if(Physics.Raycast(playerController.transform.position, -playerController.transform.up,1f))
                    break;
            }
            yield return new WaitForEndOfFrame();
        }

        isHeroicLeap = false;
        playerController.PlayerAttack = true;
        crackParticle.Emit(1);
        Instantiate(crackParticle.gameObject, endPos - new Vector3(0, 0.15f, 0), Quaternion.FromToRotation(crackParticle.transform.up, hit.normal));
    }

    Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        var mid = Vector3.Lerp(start, end, t);
        return new Vector3(mid.x, Cal(t, height) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    float Cal(float t, float height)
    {
        return -4 * height * t * t + 4 * height * t;
    }

}

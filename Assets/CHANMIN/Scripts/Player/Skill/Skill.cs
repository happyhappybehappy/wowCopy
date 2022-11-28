using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField] protected Image ImgCool;
    [SerializeField] protected TextMeshProUGUI TextCool;

    protected string skillName;
    protected bool useSkill;
    protected float coolDown;
    protected float attackRange;
    protected int getFury = 0;
    protected int requirementFury = 0;

    protected GameObject player;
    protected GameObject mountPlayer;

    protected float skillDamage;
    protected IEnumerator skill;
    protected PlayerController playerController;
    protected CharacterController characterController;
    protected PlayerFindTarget playerFindTarget;
    protected TargetManager targetManager;
    protected UIManager uimanager;

    public virtual void Awake()
    {
        playerController = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        player = FindObjectOfType<PlayerController>().gameObject;

        characterController = FindObjectOfType<PlayerController>().GetComponent<CharacterController>();
        playerFindTarget = GetComponent<PlayerFindTarget>();
        uimanager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
        targetManager = FindObjectOfType<TargetManager>().GetComponent<TargetManager>();
        Init_Ui();
    }

    public virtual IEnumerator OnSkill()
    {
        yield return null;
    }
    public virtual IEnumerator OnSkill(RaycastHit hit)
    {
        yield return null;
    }

    public virtual IEnumerator CheckCoolDown(float coolCurrent)
    {
        useSkill = false;
        TextCool.enabled = true;
        while (coolCurrent >= 1.0f)
        {
            coolCurrent -= Time.deltaTime;
            ImgCool.fillAmount = (1.0f / coolCurrent);
            TextCool.text = coolCurrent.ToString("0");
            yield return new WaitForFixedUpdate();
        }
        TextCool.enabled = false;
        useSkill = true;
    }

    public void Init_Ui()
    {
        ImgCool.type = Image.Type.Filled;
        ImgCool.fillMethod = Image.FillMethod.Radial360;
        ImgCool.fillOrigin = (int)Image.Origin360.Top;
        ImgCool.fillClockwise = false;
    }

    public virtual IEnumerator GetFuryGage(int getFuryCount, float waitTime)
    {
        for (int i = 0; i < getFuryCount; i++)
        {
            playerController.FuryGage += getFury;
            yield return new WaitForSeconds(waitTime);
        }
    }
}

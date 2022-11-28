using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header ("Player")]
    [Space (10f)]
    [SerializeField] private Slider playerHpSlider;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField] private Slider playerFurySlider;
    [SerializeField] private TextMeshProUGUI playerFuryText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private Image playerPortraits;
    [SerializeField] private TextMeshProUGUI playerNameText;

    [Header ("Target")]
    [Space(10f)]
    [SerializeField] private Slider targetHpSlider;
    [SerializeField] private TextMeshProUGUI targetHpText;
    [SerializeField] private Image targetPortraits;
    [SerializeField] private TextMeshProUGUI targetNameText;

    [Space(10f)]

    public TargetManager targetManager;
    public PlayerController playerController;

    private void Awake()
    {
        playerPortraits.sprite = playerController.Portraits;
        playerNameText.text = playerController.UnitName;
    }

    public void TargetInfoUpdate(Sprite targetPortraits, string targetName, string targetCurHP, string targetMaxHP)
    {
        this.targetPortraits.sprite = targetPortraits;
        targetNameText.text = targetName;
        targetHpText.text = targetCurHP + '/' + targetMaxHP;
    }

    public void UpdatePlayerLevelUp()
    {
        playerLevelText.text = playerController.Level.ToString();
    }

    public IEnumerator UpdateTargetHPCo()
    {
        targetHpSlider.value = Mathf.Lerp(targetHpSlider.value, targetManager.target.GetComponent<Unit>().CurHp / targetManager.target.GetComponent<Unit>().Hp, Time.time * 10);
        yield return null;
    }

    public IEnumerator UpdatePlayerHPCo()
    {
        playerHpText.text = playerController.CurHp.ToString() + '/' + playerController.Hp;
        playerHpSlider.value = Mathf.Lerp(playerHpSlider.value, playerController.CurHp / playerController.Hp, Time.time * 10);
        yield return null;
    }

    public IEnumerator UpdatePlayerFuryCo()
    {
        playerFuryText.text = playerController.FuryGage.ToString() + '/' + 100;
        playerFurySlider.value = Mathf.Lerp(playerFurySlider.value, playerController.FuryGage / 100f, Time.time * 10);
        yield return null;
    }

    public IEnumerator UpdateXPCo()
    {
        xpText.text = playerController.PlayerXP.ToString() + '/' + 10;
        xpSlider.value = Mathf.Lerp(xpSlider.value, playerController.PlayerXP / 10f, Time.time * 10);
        yield return null;
    }
}
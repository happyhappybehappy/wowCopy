using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManger : MonoBehaviour
{
    public PlayerController playerContoller;
    public GameObject levelUpParticle;

    public void LevelUP()
    {
        Instantiate(levelUpParticle, playerContoller.transform);
        switch (playerContoller.Level)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:  playerContoller.Level++;
                break;
        }
    }

    public void CompareXP(ref float xp)
    {
        if (xp < 10) return;

        ClampXP(ref xp);
        LevelUP();
    }

    public void ClampXP(ref float xp)
    {
        if (xp < 10) return;

        xp %= 10;
    }

}

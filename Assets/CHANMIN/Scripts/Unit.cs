using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [SerializeField] private Sprite portraits;
    public Sprite Portraits
    {
        get { return portraits; }
        set { portraits = value; }
    }

    [SerializeField] private string unitName;
    public string UnitName
    {
        get { return unitName; }
        set { unitName = value; }
    }

    [SerializeField] private float hp;
    public virtual float Hp => hp;

    [SerializeField] private float curHp;
    public float CurHp
    {
        get { return curHp; }
        set
        {
            curHp = value;

            if (curHp <= 0)
            {
                curHp = 0;
                Die();
            }
        }
    }

    public Unit(string unitName)
    {
        UnitName = unitName;
        CurHp = Hp;
    }

    public virtual void Die()
    { }
}

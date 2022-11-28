using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  interface ISKillActonTarget
{
    //public void UseActionSkill(Transform target);
    public void UseActionSkill(RaycastHit hit);
}

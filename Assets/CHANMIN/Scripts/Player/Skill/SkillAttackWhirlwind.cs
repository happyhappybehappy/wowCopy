using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackWhirlwind : SkillAttack
{
    private float timer = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
            other.gameObject.GetComponent<IDamagable>()?.TakeHit(damage);
    }

    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;
        if (timer >= 2f) timer = 0;

        if(timer == 0)
            if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
                other.gameObject.GetComponent<IDamagable>()?.TakeHit(damage);
    }
}

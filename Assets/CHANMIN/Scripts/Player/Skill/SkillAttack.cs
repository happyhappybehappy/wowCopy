using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack : MonoBehaviour
{
    protected LayerMask layerMask;
    [SerializeField] protected float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
            other.gameObject.GetComponent<IDamagable>()?.TakeHit(damage);
    }
}

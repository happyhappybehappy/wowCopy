using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ParticleManager : MonoBehaviour
{
    public LayerMask layerMask;
    public ParticleSystem[] particleObj;

    public void ParticleControl(Vector3 pos, RaycastHit hit)
    {
        particleObj[0].Emit(1);
        Instantiate(particleObj[0].gameObject, pos + new Vector3(0, 0.2f, 0), Quaternion.FromToRotation(particleObj[0].transform.up, hit.normal));
    }

    public void ParticleControl(GameObject target, float size)
    {
        particleObj[1].Emit(1);
        var obj = particleObj[1].main;
        obj.startSize = size;
        Instantiate(particleObj[1].gameObject, target.transform.position + new Vector3(0, -1f, 0), particleObj[1].transform.rotation);
    }
}

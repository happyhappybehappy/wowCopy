using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySecParticle : MonoBehaviour
{
    protected float timer = 0;
    public float inputTimer;

    void Update()
    {
        if (timer > inputTimer)
            Destroy(gameObject);

        timer += Time.deltaTime;
    }
}

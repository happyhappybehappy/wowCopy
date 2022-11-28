using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefab : MonoBehaviour
{
    GameObject item;
    public float lifeTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameObject.Find(item.name + "(Clone)"), lifeTime);
    }
}

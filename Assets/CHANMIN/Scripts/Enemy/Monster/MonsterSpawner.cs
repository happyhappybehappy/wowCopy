using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monster;
    public int monsterCount;
    private static MonsterSpawner instance;
    public static MonsterSpawner Instance
    {
        set { instance = value; }
        get { return instance; }
    }

    public float xPos, zPos;

    private Vector3 randVec;

    public Queue<GameObject> monsterQueue = new Queue<GameObject>();
    private void Start()
    {
        instance = this;

        for (int i = 0; i < monsterCount; i++)
        {
            GameObject tempObj = Instantiate(monster, this.gameObject.transform);
            monsterQueue.Enqueue(tempObj);
            tempObj.SetActive(false);
        }

        StartCoroutine(MonsterSpawn());
    }

    public void InsertQueue(GameObject insertObj)
    {
        monsterQueue.Enqueue(insertObj);
        insertObj.SetActive(false);
    }

    public GameObject GetQueue()
    {
        GameObject activeMonster = monsterQueue.Dequeue();
        activeMonster.SetActive(true);

        return activeMonster;
    }

    IEnumerator MonsterSpawn()
    {
        while (true)
        {
            if (monsterQueue.Count != 0)
            {
                xPos = Random.Range(-20, 20);
                zPos = Random.Range(-20, 20);
                randVec = new Vector3(xPos, 0.0f, zPos);
                GameObject tempObj = GetQueue();
                tempObj.transform.position = gameObject.transform.position + randVec;
                tempObj.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            yield return new WaitForSeconds(1f);
        }
    }

}

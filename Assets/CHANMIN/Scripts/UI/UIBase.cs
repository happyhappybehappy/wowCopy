using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    List<Transform> children;
    private void Start()
    {
        children = new List<Transform>();
        UpdateChildren();
    }

    public void UpdateChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == children.Count)
                children.Add(null);

            var child = transform.GetChild(i);

            if (child != null)
                children[i] = child;
        }
        children.RemoveRange(transform.childCount, children.Count - transform.childCount);
    }
    public int GetIndexByPosition(Transform obj, int skipIndex = -1)
    {
        int result = 0;

        for (int i = 0; i < children.Count; i++)
        {
            if (obj.position.x < children[i].position.x)
                break;

            else if (skipIndex != i)
                result++;
        }
        return result;
    }

    public void SwapObj(int prev, int next)
    {
        Central.SwapObj(children[prev], children[next]);
        UpdateChildren();
    }

    public void InsertObj(Transform obj, int index)
    {
        children.Add(obj);
        obj.SetSiblingIndex(index);
        UpdateChildren();
    }
}

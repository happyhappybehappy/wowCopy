using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Central : MonoBehaviour
{
    public Transform invisible;

    List<UIBase> uiBases;

    UIBase workingUIBase;
    int oriIndex;

    private void Start()
    {
        uiBases = new List<UIBase>();

        var uiArrs = transform.GetComponentsInChildren<UIBase>();

        for (int i = 0; i < uiArrs.Length; i++)
        {
            uiBases.Add(uiArrs[i]);
        }
    }

    bool ContainPos(RectTransform rt, Vector2 pos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rt, pos);
    }

    public static void SwapObj(Transform prev, Transform next)
    {
        Transform prevParent = prev.parent;
        Transform nextParent = next.parent;

        int prevIndex = prev.GetSiblingIndex();
        int nextIndex = next.GetSiblingIndex();

        prev.SetParent(nextParent);
        prev.SetSiblingIndex(nextIndex);

        next.SetParent(prevParent);
        next.SetSiblingIndex(prevIndex);
    }



    void SwapObjInHierarchy(Transform prev, Transform next)
    {
        SwapObj(prev, next);
        uiBases.ForEach(t => t.UpdateChildren());
    }

    void BeginDrag(Transform obj)
    {
        //Debug.Log("BeginDrag : " + obj.name);
        workingUIBase = uiBases.Find(t => ContainPos(t.transform as RectTransform, obj.position));
        oriIndex = obj.GetSiblingIndex();

        SwapObjInHierarchy(invisible, obj);
    }


    void Drag(Transform obj)
    {
        //Debug.Log("Drag : " + obj.name);
        var whichUIBaseObj = uiBases.Find(t => ContainPos(t.transform as RectTransform, obj.position));


        if (whichUIBaseObj == null)
        {
            bool updateChildren = transform != invisible.parent;

            invisible.SetParent(transform);

            if (updateChildren)
            {
                uiBases.ForEach(t => t.UpdateChildren());
            }

        }

        else
        {
            bool insert = invisible.parent == transform;
            if (insert)
            {
                int index = whichUIBaseObj.GetIndexByPosition(obj);

                invisible.SetParent(whichUIBaseObj.transform);

                whichUIBaseObj.InsertObj(invisible, index);

            }

            else
            {
                int invisibleObjIndex = invisible.GetSiblingIndex();
                int targetIndex = whichUIBaseObj.GetIndexByPosition(obj, invisibleObjIndex);

                if (invisibleObjIndex != targetIndex)
                    whichUIBaseObj.SwapObj(invisibleObjIndex, targetIndex);
            }  
        }
    }

    void EndDrag(Transform obj)
    {
        // Debug.Log("EndDrag : " + obj.name);
        if (invisible.parent == transform)
        {
            workingUIBase.InsertObj(obj, oriIndex);
            workingUIBase = null;
            oriIndex = -1;
        }


        SwapObjInHierarchy(invisible, obj);



    }


}

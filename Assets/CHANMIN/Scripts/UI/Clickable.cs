using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public abstract class Clickable : MonoBehaviour
{
    public GameObject target;
    public Vector3 targetPos;
    public Vector3 targetInteractablePos;
    protected bool isStartCoroutine = false;
    public abstract void OnTargetLeft();
    public abstract void OnTargetRight();
    public abstract void OnMouseOver();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViewDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> OnFindTarget;
    [SerializeField] public GameObject target;

    [Header("View Detector")]
    [SerializeField] private float viewRadius = 1f;
    [SerializeField, Range(0f, 360f)] private float viewAngle = 30f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask ObstacleMask;

    private void Update()
    {
        //FindTarget();
    }

    public void FindTarget()
    {

        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 dirToTarget = (targets[i].transform.position - transform.position).normalized;

            if (Vector3.Dot(transform.forward, dirToTarget) < Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad))
                continue;

            float disToTarget = Vector3.Distance(transform.position, targets[i].transform.position);
            if (Physics.Raycast(transform.position, dirToTarget, disToTarget, ObstacleMask))
                continue;

            Debug.DrawRay(transform.position, dirToTarget * disToTarget, Color.red);

            target = targets[i].gameObject;
            return;
        }
        target = null;
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);
        Vector3 lefttDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);

        Debug.DrawRay(transform.position, lookDir * viewRadius, Color.green);
        Debug.DrawRay(transform.position, rightDir * viewRadius, Color.blue);
        Debug.DrawRay(transform.position, lefttDir * viewRadius, Color.blue);
    }



}

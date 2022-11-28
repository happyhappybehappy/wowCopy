using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform fallCheckPoint;
    [SerializeField, Range(0, 1000)] private float distance;
    [SerializeField, Range(0, 10)] private float fallDistance;
    [SerializeField] private float hitDistance;
    public bool IsGrounded { get; private set; }
    public bool IsFall     { get; private set; }

    public RaycastHit hit;

    // hit.distance // vector3 distance에서 일정 거리 차이가 나면 스킵

    private void Start()
    {
      //  ray = new Ray(transform.position, -transform.up);
    }


    private void Update()
    {
        // IsGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, distance, layerMask);
        IsGrounded = Physics.SphereCast(groundCheckPoint.position, 2f, Vector3.down, out hit, distance, layerMask);
        IsFall = IsCheckGrounded();
    }

    public bool IsCheckGrounded()
    {
        if (hit.distance < hitDistance)
            return true;

        var ray = new Ray(groundCheckPoint.position + Vector3.up * 0.1f, Vector3.down);

        return Physics.Raycast(ray, fallDistance, layerMask);
    }
}


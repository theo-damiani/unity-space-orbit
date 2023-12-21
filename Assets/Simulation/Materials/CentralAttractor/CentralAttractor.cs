using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CentralAttractor : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3Reference relativePosToTarget;
    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateLineRenderer();
    }

    public void InitAttractor()
    {
        SetAttractorPosition();
        UpdateLineRenderer();
    }

    public void SetAttractorPosition()
    {
        transform.position = target.position + relativePosToTarget.Value;
    }

    public void UpdateLineRenderer()
    {
        if (lr)
        {
            lr.positionCount = 2;

            lr.SetPositions(new [] {
                transform.position,
                target.position
            });
        }
    }
}

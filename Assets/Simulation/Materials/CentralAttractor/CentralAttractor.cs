using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CentralAttractor : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private FloatReference radiusLength;
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
        transform.position = target.position + target.Find("RocketObject").transform.right*radiusLength.Value;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CentralAttractor : MonoBehaviour
{
    [SerializeField] private Transform target;
    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateLineRenderer();
    }

    public void UpdateLineRenderer()
    {
        lr.positionCount = 2;

        lr.SetPositions(new [] {
            transform.position,
            target.position
        });
    }
}

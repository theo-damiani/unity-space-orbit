using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(fileName = "gravitationalMotion", menuName="Motion / Gravitational Motion")]
public class GravitationalMotion : Motion
{
    [SerializeField] private FloatReference strandardGravitationalParam; // mu = G * M = approx G(M+m) if m <<< M
    [SerializeField] private Vector3Reference attractorBodyPos;
    [SerializeField] private Vector3Reference currentGravitationalForce;
    [SerializeField] private GameEvent collisionWithCelestialBody;

    public override void InitMotion(Rigidbody rigidbody)
    {

    }

    public override void ApplyMotion(Rigidbody rigidbody)
    {
        Vector3 radius = GetRadius(rigidbody.transform);
        float radiusSqr = radius.sqrMagnitude;
        if (radiusSqr <= 0.5f)
        {
            collisionWithCelestialBody.Raise();
            SetVectorRepresentation(currentGravitationalForce, Vector3.zero);
            return;
        }
        float force = strandardGravitationalParam.Value/radiusSqr;
        
        if (force < 0.01f)
        {
            SetVectorRepresentation(currentGravitationalForce, Vector3.zero);
            return;
        }
        Vector3 forceVector = force*radius.normalized;
        rigidbody.AddForce(forceVector, ForceMode.Acceleration);
        SetVectorRepresentation(currentGravitationalForce, forceVector);
    }

    private Vector3 GetRadius(Transform t)
    {
        return attractorBodyPos.Value - t.localPosition;
    }

    public void SetVectorRepresentation(Vector3Reference vectorRef, Vector3 newComponents)
    {
        if (vectorRef.Value == newComponents)
        {
            return;
        }
        vectorRef.Value = newComponents;
    }
}
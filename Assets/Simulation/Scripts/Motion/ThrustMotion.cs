using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "thrustMotion", menuName="Motion / Thrust Motion")]
public class ThrustMotion : Motion
{
    public Vector3Reference force;
    [SerializeField] private float forceScale;
    public override void InitMotion(Rigidbody rigidbody)
    {
    }

    public override void ApplyMotion(Rigidbody rigidbody)
    {
        //return;
        rigidbody.AddForce(force.Value*forceScale, ForceMode.Force);
    }
}

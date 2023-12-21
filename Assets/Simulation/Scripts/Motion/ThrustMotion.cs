using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "thrustMotion", menuName="Motion / Thrust Motion")]
public class ThrustMotion : Motion
{
    [SerializeField] private float forceScale;
    public override void InitMotion(Rigidbody rigidbody)
    {
        // rigidbody.velocity = Vector3.zero;
        // rigidbody.AddForce(velocity.Value, ForceMode.VelocityChange);
    }

    public override void ApplyMotion(Rigidbody rigidbody)
    {
        //return;
        rigidbody.AddForce(velocity.Value*forceScale, ForceMode.Force);
    }
}

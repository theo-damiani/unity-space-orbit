using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "uniformMotion", menuName="Motion / Uniform Motion")]
public class UniformMotion : Motion
{
    public override void InitMotion(Rigidbody rigidbody)
    {
        // rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(velocity.Value, ForceMode.VelocityChange);
    }

    public override void ApplyMotion(Rigidbody rigidbody)
    {
        //return;
        //rigidbody.AddForce(velocity.Value, ForceMode.Force);
    }
}

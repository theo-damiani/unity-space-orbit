using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  Motion : ScriptableObject, IMotion
{
    public Vector3Reference velocity;
    public abstract void ApplyMotion(Rigidbody rigidbody);
    public abstract void InitMotion(Rigidbody rigidbody);
}

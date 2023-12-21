using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMotion
{
    void ApplyMotion(Rigidbody rigidbody);
    void InitMotion(Rigidbody rigidbody);
}

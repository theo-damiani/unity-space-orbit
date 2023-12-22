using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindVectorRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3Variable vector;

    public void SetRotationFromTarget()
    {
        float angle =  Vector3.Angle(target.up , vector.Value);

        vector.Value = Quaternion.AngleAxis(angle, Vector3.up) * vector.Value;
    }

}

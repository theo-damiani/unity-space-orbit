using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindVectorRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3Variable vector;

    public void SetRotationFromTarget()
    {
        Vector3 relativePos =  vector.Value - target.position;
        vector.Value = Quaternion.LookRotation(relativePos, Vector3.up) * vector.Value;
    }

}

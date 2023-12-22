using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindVectorRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3Variable vector;
    [SerializeField] private VectorLabel label;

    public void SetRotationFromTarget()
    {
        float angle =  Vector3.SignedAngle(vector.Value, target.up, Vector3.up);
        vector.Value = Quaternion.AngleAxis(angle, Vector3.up) * vector.Value;

        label.UpdateSprite();
    }

}

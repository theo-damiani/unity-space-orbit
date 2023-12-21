using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "centralMotion", menuName="Motion / Central Motion")]
public class CentralMotion : Motion
{
    public Vector3Reference relativeCenter;
    public FloatReference angularvelocity;
    [SerializeField] private Vector3Reference currentCentralForce;
    private Vector3 center;
    public override void InitMotion(Rigidbody rigidbody)
    {
        // Init center of attraction:
        center = rigidbody.transform.position + relativeCenter.Value;

        // Cicular mode : 
        Vector3 projectedVelocity = ProjectVelocityAlongTangent(rigidbody);
        float radiusMagn = GetRadius(rigidbody.transform).magnitude;
        if (radiusMagn==0)
        {
            rigidbody.velocity = Vector3.zero;
            angularvelocity.Value = 0f;
            return;
        }
        angularvelocity.Value = projectedVelocity.magnitude/radiusMagn;
        rigidbody.velocity = projectedVelocity;
        //rigidbody.AddForce(ProjectVelocityAlongTangent(rigidbody), ForceMode.VelocityChange);

        // Thrust mode, more real:
        // angularvelocity.Value = GetAngularVelocity(rigidbody);
    }

    public override void ApplyMotion(Rigidbody rigidbody)
    {
        ApplyWithUnityPhysics(rigidbody);
    }

    private void ApplyWithUnityPhysics(Rigidbody rigidbody)
    {
        // Centripetal Force:
        Vector3 centripetalAcceleration = angularvelocity.Value * angularvelocity.Value * GetRadius(rigidbody.transform);

        // float velocitySqr = rigidbody.velocity.sqrMagnitude;
        // Vector3 centripetalAcceleration = GetRadius(rigidbody.transform);
        // centripetalAcceleration.x = velocitySqr / centripetalAcceleration.x;
        // centripetalAcceleration.y = velocitySqr / centripetalAcceleration.y;
        // centripetalAcceleration.z = velocitySqr / centripetalAcceleration.z;

        rigidbody.AddForce(centripetalAcceleration, ForceMode.Acceleration);
        SetVectorRepresentation(currentCentralForce, centripetalAcceleration);
    }

    public Vector3 ProjectVelocityAlongTangent(Rigidbody rigidbody)
    {
        Vector3 radius = GetRadius(rigidbody.transform);
        Vector3 velocityDirectionInit = (Quaternion.Euler(0, -90, 0) * radius).normalized;

        return Vector3.Project(rigidbody.velocity, velocityDirectionInit);
    }

    public Vector3 GetVelocityFromAngularSpeed(Transform transform)
    {
        Vector3 radius = GetRadius(transform);
        Vector3 velocityDirectionInit = (Quaternion.Euler(0, -90, 0) * radius).normalized;
        Vector3 newVelocity = angularvelocity.Value*radius.magnitude*velocityDirectionInit;
        // newVelocity.y = 0;
        return newVelocity;
    }

    private float GetAngularVelocity(Rigidbody rigidbody)
    {
        if (rigidbody.velocity == Vector3.zero)
        {
            return 0f;
        }
        Vector3 radius = GetRadius(rigidbody.transform);
        float velocitySign = Mathf.Sign(Vector3.Cross(rigidbody.velocity, radius).y);

        Vector3 centerWithSameY = new Vector3(center.x, rigidbody.transform.localPosition.y, center.z);
        radius = Vector3.Project(radius, centerWithSameY - rigidbody.transform.localPosition);
        
        return velocitySign * rigidbody.velocity.magnitude / radius.magnitude;
    }

    private Vector3 GetRadius(Transform t)
    {
        return center - t.localPosition;
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
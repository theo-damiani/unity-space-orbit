using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(fileName = "centralMotion", menuName="Motion / Central Motion")]
public class CentralMotion : Motion
{
    public FloatReference angularVelocity;
    public FloatReference radiusLength;
    [SerializeField] private Vector3Reference currentCentralForce;
    private Vector3 center;
    private Transform meshTransform;
    private Vector3 angularVector;

    public override void InitMotion(Rigidbody rigidbody)
    {
        SetVectorRepresentation(currentCentralForce, Vector3.zero);
        
        meshTransform = rigidbody.transform.Find("RocketObject").transform;
        // Init center of attraction:
        center = rigidbody.transform.position + meshTransform.right*radiusLength.Value;


        Vector3 radius = GetRadius(rigidbody.transform);
        if (radiusLength.Value==0)
        {
            rigidbody.velocity = Vector3.zero;
            angularVelocity.Value = 0f;
            return;
        }
        Vector3 projectedVelocity = ProjectVelocityAlongTangent(rigidbody);
        angularVelocity.Value = projectedVelocity.magnitude / radiusLength.Value;

        angularVector = (angularVelocity.Value * Vector3.Cross(projectedVelocity, radius)).normalized;
        rigidbody.velocity = projectedVelocity;

        // SetVectorRepresentation(currentCentralForce, Vector3.zero);
        // meshTransform = rigidbody.transform.Find("RocketObject").transform;
        // // Init center of attraction:
        // //center = rigidbody.transform.position + relativeCenter.Value;
        // center = rigidbody.transform.position + meshTransform.right*radiusLength.Value;

        // // Cicular mode : 
        // Vector3 projectedVelocity = ProjectVelocityAlongTangent(rigidbody);
        // float radiusMagn = GetRadius(rigidbody.transform).magnitude;
        // if (radiusMagn==0)
        // {
        //     rigidbody.velocity = Vector3.zero;
        //     angularVelocity.Value = 0f;
        //     return;
        // }
        // angularVelocity.Value = projectedVelocity.magnitude/radiusMagn;
        // rigidbody.velocity = projectedVelocity;

        //rigidbody.AddForce(ProjectVelocityAlongTangent(rigidbody), ForceMode.VelocityChange);

        // Thrust mode, more real:
        // angularVelocity.Value = GetangularVelocity(rigidbody);
    }

    public override void ApplyMotion(Rigidbody rigidbody)
    {
        ApplyWithUnityPhysics(rigidbody);
    }

    private void ApplyWithUnityPhysics(Rigidbody rigidbody)
    {
        // // Centripetal Force:
        Vector3 centripetalAcceleration = angularVelocity.Value * angularVelocity.Value * GetRadius(rigidbody.transform);

        rigidbody.AddForce(centripetalAcceleration, ForceMode.Acceleration);
        SetVectorRepresentation(currentCentralForce, centripetalAcceleration);
        meshTransform.Rotate(angularVector, Mathf.Rad2Deg * angularVelocity.Value*Time.fixedDeltaTime, Space.World);
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
        Vector3 newVelocity = angularVelocity.Value*radius.magnitude*velocityDirectionInit;

        return newVelocity;
    }

    private float GetangularVelocity(Rigidbody rigidbody)
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
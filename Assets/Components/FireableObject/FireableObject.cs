using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FirableObject : MonoBehaviour
{
    public IEnumerator StopObjectAfterTime(float t)
    {
        yield return new WaitForSeconds(t);
        StopObject();
    }
    private void StopObject()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    }
    public void SetupTrailRenderer()
    {
        TrailRenderer trailRenderer;
        if(!TryGetComponent<TrailRenderer>(out trailRenderer))
        {
            trailRenderer = this.AddComponent<TrailRenderer>();
        }
        // Trail Renderer On:
        trailRenderer.enabled = true;
        trailRenderer.time = float.PositiveInfinity;
        AnimationCurve curve = new AnimationCurve();
        float curveWidth = 0.3f;
        curve.AddKey(0f, curveWidth);
        curve.AddKey(1f, curveWidth);
        trailRenderer.widthCurve = curve;
    }

    public void Fire(Vector3 initLocalPosition, Quaternion initLocalRotation, Vector3 initVelocity, bool useGravity)
    {
        transform.SetLocalPositionAndRotation(initLocalPosition, initLocalRotation);
        
        GetComponent<Rigidbody>().AddForce(initVelocity, ForceMode.VelocityChange);

        GetComponent<Rigidbody>().useGravity = useGravity;
        GetComponent<Rigidbody>().isKinematic = false;
    }


    public IEnumerator FireWithForce(Vector3 initLocalPosition, Quaternion initLocalRotation, Vector3 initVelocity, bool useGravity, Vector3 force, float forceTimer)
    {
        float time = 0;
        
        transform.SetLocalPositionAndRotation(initLocalPosition, initLocalRotation);
        
        GetComponent<Rigidbody>().AddForce(initVelocity, ForceMode.VelocityChange);

        GetComponent<Rigidbody>().useGravity = useGravity;
        GetComponent<Rigidbody>().isKinematic = false;

        while (time <= forceTimer)
        {
            GetComponent<Rigidbody>().AddForce(force);
            time += Time.fixedDeltaTime;
            yield return null;
        }
    }
}
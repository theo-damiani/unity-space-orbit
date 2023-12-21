using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PausableRigidbody : MonoBehaviour {

    private Rigidbody _rigidBody;
    void Awake () 
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // ================== Pausable logic ================== //

    private Vector3 _pausedVelocity;
    private Vector3 _pausedAngularVelocity;

    public void Pause() 
    {
        _pausedVelocity = _rigidBody.velocity;
        _pausedAngularVelocity = _rigidBody.angularVelocity;
        _rigidBody.isKinematic = true;

        SetVelocityVector();
    }

    public void Resume() 
    {
        _rigidBody.isKinematic = false;
        _rigidBody.AddForce(_pausedVelocity, ForceMode.VelocityChange);
        _rigidBody.AddTorque(_pausedAngularVelocity, ForceMode.VelocityChange);
    }

    // ================== Velocity vector bind logic ================== //

    [SerializeField] private DraggableVector velocityVector;

    public void OnEnable()
    {
        if (velocityVector)
        {
            velocityVector.GetHeadClickZone().OnZoneMouseUp += SetPausedVelocity;
        }
    }

    private void OnDisable()
    {
        if (velocityVector)
        {
            velocityVector.GetHeadClickZone().OnZoneMouseUp -= SetPausedVelocity;
        }
    }

    private void SetPausedVelocity(VectorClickZone clickZone)
    {
        _pausedVelocity = velocityVector.components.Value;
    }

    private void SetVelocityVector()
    {
        velocityVector.components.Value = _pausedVelocity;
        velocityVector.Redraw(); 
    }
}

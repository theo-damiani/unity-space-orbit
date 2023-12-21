using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VectorRigidbodyBind : MonoBehaviour
{
    [SerializeField] private Vector3Variable velocityVector;
    [SerializeField] private DraggableVector vector;
    private Rigidbody rb;
    private bool isPaused;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isPaused = false;
    }

    void Update()
    {
        if ((!vector.IsDragged()) && (!isPaused))
        {
            SetVectorVelocity();
        }
    }

    public void OnEnable()
    {
        if (vector)
        {
            vector.GetHeadClickZone().OnZoneMouseUp += SetRigidbodyVelocity;
        }
    }

    private void OnDisable()
    {
        if (vector)
        {
            vector.GetHeadClickZone().OnZoneMouseUp -= SetRigidbodyVelocity;
        }
    }

    public void SetRigidbodyVelocity(VectorClickZone clickZone)
    {
        if (!rb.isKinematic)
            rb.velocity = velocityVector.Value;
    }

    public void SetVectorVelocity()
    {
        if (velocityVector.Value == rb.velocity)
        {
            return;
        }
        velocityVector.Value = rb.velocity;
        vector.Redraw();
    }

    public void SetIsPaused(bool value)
    {
        isPaused = value;
    }
}

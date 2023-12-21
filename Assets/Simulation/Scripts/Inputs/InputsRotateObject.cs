using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsRotateObject : MonoBehaviour
{
    public BoolReference isUpKeyActive;
    public BoolReference isDownKeyActive;
    public BoolReference isLeftKeyActive;
    public BoolReference isRightKeyActive;
    [SerializeField] private List<Transform> transforms;
    [SerializeField] private Vector3 rotationLocalAxis;
    [SerializeField] private float rotationScale;
    [SerializeField] private Vector3Variable vector;
    private Vector3 localRotationRight;
    private Vector3 localRotationUp;

    public void Start()
    {
        enabled = isUpKeyActive.Value | isDownKeyActive.Value | isLeftKeyActive.Value | isRightKeyActive.Value;
        localRotationRight = rotationLocalAxis*rotationScale;
        localRotationUp = Vector3.right*rotationScale;
    }
    void Update()
    {
        if (isRightKeyActive.Value && Input.GetKey(KeyCode.RightArrow))
        {
            RotateAll(localRotationRight);
        }
        if (isLeftKeyActive.Value && Input.GetKey(KeyCode.LeftArrow))
        {
            RotateAll(-localRotationRight);
        }
        if (isUpKeyActive.Value && Input.GetKey(KeyCode.UpArrow))
        {
            RotateAll(localRotationUp);
        }
        if (isDownKeyActive.Value && Input.GetKey(KeyCode.DownArrow))
        {
            RotateAll(-localRotationUp);
        }
    }

    public void PressKeyUp()
    {
        if (isUpKeyActive.Value)
            RotateAll(localRotationUp);
    }

    public void PressKeyDown()
    {
        if (isDownKeyActive.Value)
            RotateAll(-localRotationUp);
    }

    public void PressKeyLeft()
    {
        if (isLeftKeyActive.Value)
            RotateAll(-localRotationRight);
    }

    public void PressKeyRight()
    {
        if (isRightKeyActive.Value)
            RotateAll(localRotationRight);
    }

    private void RotateAll(Vector3 rot)
    {
        foreach (Transform t in transforms)
        {
            t.Rotate(rot*Time.deltaTime);
            Vector3 worldRot = t.TransformVector(rot);
            vector.Value = Quaternion.Euler(worldRot*Time.deltaTime) * vector.Value;
        }
    }
}

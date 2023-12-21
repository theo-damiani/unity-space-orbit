using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour
{
    [SerializeField] private Transform initPosition;
    [SerializeField] private Vector3Reference initVelocity;
    [SerializeField] private GameObject prefabToFire;
    private bool useGravity;

    public void FireObjectFromOrigin()
    {
        GameObject newObject = Instantiate(prefabToFire, transform);

        if (!newObject.TryGetComponent<FirableObject>(out FirableObject fo))
        {
            fo = newObject.AddComponent<FirableObject>();
        }

        fo.Fire(initPosition.localPosition, initPosition.localRotation,
            initVelocity.Value, useGravity);
    }

    public void EnableGravity()
    {
        useGravity = true;
    }

    public void DisableGravity()
    {
        useGravity = false;
    }
}

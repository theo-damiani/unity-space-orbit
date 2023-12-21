using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "RopeData", menuName = "Components / Rope")]
public class RopeData : ScriptableObject
{
    public GameObject ropeSegmentPrefab;

    [Header("Rope Variables Definition")]
    [SerializeField] private float ropeLength;
    [field: SerializeField] public Vector3 RopeSegmentScale { get; private set; }
    [SerializeField] public bool RopeUseGravity;

    // Actions:
    public static event System.Action OnRopeLengthUpdate;

    /* *********** Properties *********** */

    public float RopeLength 
    {
        get {return ropeLength;}

        set
        {
            ropeLength = value;
            OnRopeLengthUpdate?.Invoke();
        }
    }
}

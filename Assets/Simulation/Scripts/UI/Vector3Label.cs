using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


[ExecuteInEditMode, RequireComponent(typeof(TextMeshProUGUI))]
public class Vector3Label : MonoBehaviour
{
    [SerializeField] private Vector3Reference vector3Variable;
    [SerializeField] private Vector3 defaultVariable;
    [SerializeField] private BoolReference boolReference;
    [SerializeField] private BoolReference useDefaultOnBool;
    [SerializeField] private int fontSize;
    [SerializeField] private TextMeshProUGUI labelX;
    [SerializeField] private TextMeshProUGUI labelY;
    [SerializeField] private TextMeshProUGUI labelZ;
    
    void Start()
    {
        labelX.fontSize = fontSize;
        labelY.fontSize = fontSize;
        labelZ.fontSize = fontSize;
        SetText();
    }

    public void SetText()
    {
        if (useDefaultOnBool.Value == boolReference.Value)
        {
            labelX.text = defaultVariable.x.ToString("F2");
            labelY.text = defaultVariable.y.ToString("F2");
            labelZ.text = defaultVariable.z.ToString("F2");
        }
        else 
        {
            labelX.text = vector3Variable.Value.x.ToString("F2");
            labelY.text = vector3Variable.Value.y.ToString("F2");
            labelZ.text = vector3Variable.Value.z.ToString("F2");
        }
    }

    void OnEnable()
    {
        GameEvent gameEvent = vector3Variable.OnUpdateEvent;
        if (gameEvent)
            gameEvent.OnRaise += SetText;

        gameEvent = boolReference.OnUpdateEvent;
        if (gameEvent)
            gameEvent.OnRaise += SetText;
    }

    void OnDisable()
    {
        GameEvent gameEvent = vector3Variable.OnUpdateEvent;
        if (gameEvent)
            gameEvent.OnRaise -= SetText;

        gameEvent = boolReference.OnUpdateEvent;
        if (gameEvent)
            gameEvent.OnRaise -= SetText;
    }

    // Editor-only function that Unity calls when the script is loaded or a value changes in the Inspector.
    void OnValidate()
    {
        labelX.fontSize = fontSize;
        labelY.fontSize = fontSize;
        labelZ.fontSize = fontSize;
        SetText();
    }
}

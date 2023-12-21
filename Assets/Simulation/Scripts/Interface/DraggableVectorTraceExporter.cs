using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class ExtraVectorComponents
{
    public ExtraVectorComponents(Vector3 v)
    {
        components = v;
    }
    public Vector3 components;
}

[Serializable]
public class DraggableVectorTrace : UserTraceHolder 
{
    public DraggableVectorTrace(List<double> t, string i, UnityActionType a, List<ExtraVectorComponents> x)
    {
        time = t;
        objectId = i;
        actionType = a;
        extra = x;
    }
    public List<double> time;
    public string objectId;
    public UnityActionType actionType;
    public List<ExtraVectorComponents> extra;
}

public class DraggableVectorTraceExporter : AnalyticsExporter
{
    [SerializeField] private DraggableVector vector;
    [SerializeField] private int indexAxisForX = 0;
    [SerializeField] private int indexAxisForY = 1;
    [SerializeField] private int indexAxisForZ = 2;
    [SerializeField] private int timeRate = 1;
    private List<double> listTimes;
    private List<ExtraVectorComponents> listExtras;

    void Start()
    {
        listTimes = new List<double>();
        listExtras = new List<ExtraVectorComponents>();
    }

    void OnEnable()
    {
        if(vector)
        {
            vector.GetHeadClickZone().OnZoneMouseDown += WrapperOnPressVector;
            vector.GetHeadClickZone().OnZoneMouseUp += WrapperOnReleaseVector;
        }
    } 

    void OnDisable()
    {
        if(vector)
        {
            vector.GetHeadClickZone().OnZoneMouseDown -= WrapperOnPressVector;
            vector.GetHeadClickZone().OnZoneMouseUp -= WrapperOnReleaseVector;
        }
    }

    private void WrapperOnPressVector(VectorClickZone clickZone)
    {
        listTimes.Clear();
        listExtras.Clear();
        InvokeRepeating(nameof(AddTrace), 0f, timeRate);
    }
    private void WrapperOnReleaseVector(VectorClickZone clickZone)
    {
        SendVectorTrace();
        CancelInvoke(nameof(AddTrace));
    }

    private void AddTrace()
    {
        listTimes.Add(Math.Round(Time.timeSinceLevelLoadAsDouble, 2));
        listExtras.Add(new ExtraVectorComponents(SwitchComponentsAxis(vector.components.Value)));
    }
    private void SendVectorTrace()
    {
        DraggableVectorTrace newUserTrace = new (listTimes, vector.gameObject.name, UnityActionType.Drag, listExtras);

        SendNewTrace(newUserTrace);
    }

    private Vector3 SwitchComponentsAxis(Vector3 components)
    {
        return new Vector3(components[indexAxisForX], components[indexAxisForY], components[indexAxisForZ]);
    }
}

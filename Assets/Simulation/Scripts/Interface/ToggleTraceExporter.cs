using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class ExtraToggleValue
{
    public ExtraToggleValue(bool v)
    {
        value = v;
    }
    public bool value;
}

[Serializable]
public class ToggleUITrace : UserTraceHolder 
{
    public ToggleUITrace(double t, string i, UnityActionType a, ExtraToggleValue x)
    {
        time = t;
        objectId = i;
        actionType = a;
        extra = x;
    }
    public double time;
    public string objectId;
    public UnityActionType actionType;
    public ExtraToggleValue extra;
}
public class ToggleTraceExporter : AnalyticsExporter
{
    [SerializeField] private Button button;
    [SerializeField] private ToggleIcons toggle;

    void OnEnable()
    {
        if(button)
        {
            button.onClick.AddListener(CreatAndSendNewTrace);
        }
    } 

    void OnDisable()
    {
        if(button)
        {
            button.onClick.RemoveListener(CreatAndSendNewTrace);
        }
    }

    private void CreatAndSendNewTrace()
    {
        ToggleUITrace newUserTrace = new(Math.Round(Time.timeSinceLevelLoadAsDouble, 2), button.gameObject.name, UnityActionType.Click, new ExtraToggleValue(toggle.GetToggleValue()));

        SendNewTrace(newUserTrace);
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ButtonUITrace : UserTraceHolder 
{
    public ButtonUITrace(double t, string i, UnityActionType a, string x)
    {
        time = t;
        objectId = i;
        actionType = a;
        extra = x;
    }
    public double time;
    public string objectId;
    public UnityActionType actionType;
    public string extra;
}
public class ButtonTraceExporter : AnalyticsExporter
{
    [SerializeField] private Button button;

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

    public void CreatAndSendNewTrace()
    {
        ButtonUITrace newUserTrace = new(Math.Round(Time.timeSinceLevelLoadAsDouble, 2), button.gameObject.name, UnityActionType.Click, "");

        SendNewTrace(newUserTrace);
    }
}

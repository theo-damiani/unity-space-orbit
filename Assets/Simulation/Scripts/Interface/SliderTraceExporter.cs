using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ExtraSliderValue
{
    public ExtraSliderValue(float v)
    {
        sliderValue = v;
    }
    public float sliderValue;
}

[Serializable]
public class SliderTrace : UserTraceHolder 
{
    public SliderTrace(List<double> t, string i, UnityActionType a, List<ExtraSliderValue> x)
    {
        time = t;
        objectId = i;
        actionType = a;
        extra = x;
    }
    public List<double> time;
    public string objectId;
    public UnityActionType actionType;
    public List<ExtraSliderValue> extra;
}
public class SliderTraceExporter : AnalyticsExporter
{
    [SerializeField] private Slider slider;
    [SerializeField] private float timeRate = 0.5f;
    private EventTrigger trigger;
    private EventTrigger.Entry entryBeginDrag;
    private EventTrigger.Entry entryEndDrag;
    private List<double> listTimes;
    private List<ExtraSliderValue> listExtras;

    void Awake()
    {
        listTimes = new List<double>();
        listExtras = new List<ExtraSliderValue>();

        trigger = slider.gameObject.GetComponent<EventTrigger>();
    }

    void OnEnable()
    {
        if(slider)
        {
            if (trigger)
            {
                entryBeginDrag = new()
                {
                    eventID = EventTriggerType.BeginDrag
                };
                entryBeginDrag.callback.AddListener((data) => { WrapperOnBeginDrag(); });
                trigger.triggers.Add(entryBeginDrag);

                entryEndDrag = new()
                {
                    eventID = EventTriggerType.EndDrag
                };
                entryEndDrag.callback.AddListener((data) => { WrapperOnEndDrag(); });
                trigger.triggers.Add(entryEndDrag);
            }
        }
    } 

    void OnDisable()
    {
        if(slider)
        {
            if (trigger)
            {
                trigger.triggers.Remove(entryBeginDrag);
                trigger.triggers.Remove(entryEndDrag);
            }
        }
    }

    private void WrapperOnBeginDrag()
    {
        listTimes.Clear();
        listExtras.Clear();
        InvokeRepeating(nameof(AddTrace), 0f, timeRate);
    }
    private void WrapperOnEndDrag()
    {
        CreatAndSendNewTrace();
        CancelInvoke(nameof(AddTrace));
    }

    private void AddTrace()
    {
        listTimes.Add(Math.Round(Time.timeSinceLevelLoadAsDouble, 2));
        listExtras.Add(new ExtraSliderValue(slider.value));
    }

    private void CreatAndSendNewTrace()
    {
        SliderTrace newUserTrace = new (listTimes, slider.gameObject.name, UnityActionType.Drag, listExtras);

        SendNewTrace(newUserTrace);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ActivateFromVariable : MonoBehaviour
{
    [SerializeField] List<BoolReference> listConditions;
    [SerializeField] List<GameObject> listGameObject;
    void Start()
    {
        SetAllGameObject();
    }

    bool ReduceConditions()
    {
        return listConditions.Aggregate(true, (acc, item) => acc & item.Value);
    }

    public void SetAllGameObject()
    {
        bool b = ReduceConditions();
        foreach (GameObject go in listGameObject)
        {
            go.SetActive(b);
        }
    }

    void OnEnable()
    {
        AddConditionEventListeners();
    }

    void OnDisable()
    {
        RemoveConditionEventListeners();
    }

    private void AddConditionEventListeners()
    {
        for (int i = 0; i < listConditions.Count; i++)
        {
            GameEvent currentEvent = listConditions[i].OnUpdateEvent;
            if (currentEvent)
            {
                currentEvent.OnRaise += SetAllGameObject;
            }
        }
    }

    private void RemoveConditionEventListeners()
    {
        for (int i = 0; i < listConditions.Count; i++)
        {
            GameEvent currentEvent = listConditions[i].OnUpdateEvent;
            if (currentEvent)
            {
                currentEvent.OnRaise -= SetAllGameObject;
            }
        }
    }
}

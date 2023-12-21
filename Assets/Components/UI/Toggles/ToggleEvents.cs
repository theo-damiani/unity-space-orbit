using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEvents : MonoBehaviour
{
    [SerializeField] private GameEvent firstEvent;
    [SerializeField] private GameEvent secondEvent;

    [SerializeField] private bool raiseEventAtStart;
    [SerializeField] private int eventLastRaised;

    void Start()
    {
        if (raiseEventAtStart)
            RaiseEvent();
    }

    public void ToggleAndRaiseEvent()
    {
        eventLastRaised = (eventLastRaised+1)%2;
        RaiseEvent();
    }

    private void RaiseEvent()
    {
        if (eventLastRaised==0)
        {
            firstEvent.Raise();
        }
        else
        {
            secondEvent.Raise();
        }
    }
}

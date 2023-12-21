using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Game Event", menuName = "Event System/Game Event", order = 0)]
public class GameEvent : ScriptableObject
{
	private List<GameEventListener> listeners = new List<GameEventListener>();
    public event Action OnRaise;

    public void Raise()
    {
        OnRaise?.Invoke();
        
        for(int i = listeners.Count -1; i >= 0; i--)
            listeners[i].OnEventRaised();
    }

    public void RegisterListener(GameEventListener listener)
    {   
        listeners.Add(listener); 
    }

    public void UnregisterListener(GameEventListener listener)
    { 
        listeners.Remove(listener); 
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallEvents : MonoBehaviour
{
    public BoolReference isActive;
    public KeyCode key;
    public UnityEvent responseOnKeyDown;
    public UnityEvent responseOnKeyUp;

    public void Start()
    {
        enabled = isActive.Value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            responseOnKeyDown.Invoke();
        }
        else if (Input.GetKeyUp(key))
        {
            responseOnKeyUp.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ToggleStartActivation : MonoBehaviour
{
    [SerializeField] private Toggle toggleOn;
    [SerializeField] private Toggle toggleOff;

    public void SetToggleVisibility(bool value)
    {
        toggleOn.isOn = value;
        toggleOff.isOn = !value;
    }
}

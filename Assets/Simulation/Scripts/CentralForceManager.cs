using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CentralForceManager : MonoBehaviour
{
    [SerializeField] private GameObject UpArrow;
    [SerializeField] private GameObject LeftArrow;
    [SerializeField] private GameObject RightArrow;
    [SerializeField] private GameObject DownArrow;
    [SerializeField] private InputsRotateObject inputsRotate;
    [SerializeField] private GameObject thrustBtn;
    [SerializeField] private CallEvents thrustSpaceManager;
    [SerializeField] private DraggableVector velocityVector;

    private bool prevInputsRotateState = false;
    private bool prevthrustSpaceManagerState = false;
    private bool prevvelocityVectorState = false;


    public void SwitchControlsOff()
    {
        prevInputsRotateState = inputsRotate.enabled;
        prevthrustSpaceManagerState = thrustSpaceManager.enabled;
        prevvelocityVectorState = velocityVector.interactable;

        UpArrow.GetComponent<Button>().interactable = false;
        UpArrow.GetComponent<KeyArrowPlayer>().enabled = false;
        LeftArrow.GetComponent<Button>().interactable = false;
        LeftArrow.GetComponent<KeyArrowPlayer>().enabled = false;
        RightArrow.GetComponent<Button>().interactable = false;
        RightArrow.GetComponent<KeyArrowPlayer>().enabled = false;
        DownArrow.GetComponent<Button>().interactable = false;
        DownArrow.GetComponent<KeyArrowPlayer>().enabled = false;

        inputsRotate.enabled = false;

        thrustBtn.GetComponent<EventTrigger>().enabled = false;
        thrustBtn.GetComponent<Button>().interactable = false;
        thrustSpaceManager.enabled = false;

        velocityVector.SetInteractable(false);
    }

    public void SwitchControlsOn()
    {
        UpArrow.GetComponent<Button>().interactable = true;
        UpArrow.GetComponent<KeyArrowPlayer>().enabled = true;
        LeftArrow.GetComponent<Button>().interactable = true;
        LeftArrow.GetComponent<KeyArrowPlayer>().enabled = true;
        RightArrow.GetComponent<Button>().interactable = true;
        RightArrow.GetComponent<KeyArrowPlayer>().enabled = true;
        DownArrow.GetComponent<Button>().interactable = true;
        DownArrow.GetComponent<KeyArrowPlayer>().enabled = true;

        inputsRotate.enabled = prevInputsRotateState;

        thrustBtn.GetComponent<EventTrigger>().enabled = true;
        thrustBtn.GetComponent<Button>().interactable = true;
        thrustSpaceManager.enabled = prevthrustSpaceManagerState;

        velocityVector.SetInteractable(prevvelocityVectorState);
    }
}

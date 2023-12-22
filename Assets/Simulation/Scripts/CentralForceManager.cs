using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CentralForceManager : MonoBehaviour
{
    [SerializeField] private CentralAttractor attractor;
    [SerializeField] private Vector forceVector;
    [SerializeField] private GameObject LeftArrow;
    [SerializeField] private GameObject RightArrow;
    [SerializeField] private InputsRotateObject inputsRotate;
    [SerializeField] private GameObject thrustBtn;
    [SerializeField] private CallEvents thrustSpaceManager;
    [SerializeField] private DraggableVector velocityVector;

    private bool prevInputsRotateState = false;
    private bool prevthrustSpaceManagerState = false;
    private bool prevvelocityVectorState = false;

    public void Start()
    {
        prevInputsRotateState = inputsRotate.enabled;
        prevthrustSpaceManagerState = thrustSpaceManager.enabled;
        prevvelocityVectorState = velocityVector.interactable;
    }

    public void SwitchControlsOff()
    {

        LeftArrow.GetComponent<Button>().interactable = false;
        LeftArrow.GetComponent<KeyArrowPlayer>().enabled = false;
        RightArrow.GetComponent<Button>().interactable = false;
        RightArrow.GetComponent<KeyArrowPlayer>().enabled = false;

        inputsRotate.enabled = false;

        thrustBtn.GetComponent<EventTrigger>().enabled = false;
        thrustBtn.GetComponent<Button>().interactable = false;
        thrustSpaceManager.enabled = false;

        velocityVector.SetInteractable(false);

        attractor.InitAttractor();
        attractor.gameObject.SetActive(true);
        forceVector.components.Value = Vector3.zero;
        forceVector.Redraw();
        forceVector.gameObject.SetActive(true);
    }

    public void SwitchControlsOn()
    {
        LeftArrow.GetComponent<Button>().interactable = true;
        LeftArrow.GetComponent<KeyArrowPlayer>().enabled = true;
        RightArrow.GetComponent<Button>().interactable = true;
        RightArrow.GetComponent<KeyArrowPlayer>().enabled = true;

        inputsRotate.enabled = prevInputsRotateState;

        thrustBtn.GetComponent<EventTrigger>().enabled = true;
        thrustBtn.GetComponent<Button>().interactable = true;
        thrustSpaceManager.enabled = prevthrustSpaceManagerState;

        velocityVector.SetInteractable(prevvelocityVectorState);

        attractor.gameObject.SetActive(false);
        forceVector.gameObject.SetActive(false);
    }
}

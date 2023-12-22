using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;

public class AppManager : Singleton<AppManager>
{
    [Header("Affordances")]
    [SerializeField] private Affordances defaultAffordances;
    private Affordances currentAffordances;

    [Header("Camera")]
    [SerializeField] private CameraManager mainCamera;
    [SerializeField] private RectTransform cameraControls;
    [SerializeField] private ToggleIcons cameraLockingToggle;
    [SerializeField] private RectTransform cameraZoomSlider;

    [Header("Main App Controls")]
    [SerializeField] private RectTransform playButton;
    [SerializeField] private RectTransform resetButton;
    [SerializeField] private BoolVariable isResetEnable;
    [SerializeField] private RectTransform metaPanel;

    [Header("Rocket Variables")]
    [SerializeField] private GameObject rocket;
    [SerializeField] private BoolVariable rocketIsInteractiveUp;
    [SerializeField] private BoolVariable rocketIsInteractiveDown;
    [SerializeField] private BoolVariable rocketIsInteractiveLeft;
    [SerializeField] private BoolVariable rocketIsInteractiveRight;
    [SerializeField] private Vector3Variable rocketVelocity;
    [SerializeField] private GameObject rocketVelocityVector;
    [SerializeField] private BoolVariable showVelocityEquation;
    [SerializeField] private GameObject velocityLabel;
    [SerializeField] private BoolVariable showRocketPath;
    [SerializeField] private RectTransform showRocketPathToggle;

    [Header("Thrust Variables")]
    [SerializeField] private BoolVariable thrustIsActive;
    [SerializeField] private BoolVariable thrustIsInteractive;
    [SerializeField] private Vector3Variable thrustForce;
    [SerializeField] private BoolVariable thrustShowVector;
    [SerializeField] private GameObject thrustShowLabel;
    [SerializeField] private BoolVariable thrustShowEquation;

    [Header("Central Force Variables")]
    [SerializeField] private BoolVariable centralForceIsActive;
    [SerializeField] private BoolVariable centralForceIsInteractive;
    [SerializeField] private FloatVariable centralRadius;
    [SerializeField] private BoolVariable centralForceShowVector;
    [SerializeField] private GameObject centralForceShowLabel;
    [SerializeField] private BoolVariable centralForceShowEquation;
    [SerializeField] private ToggleIcons centralForceToggle;
    [SerializeField] private CentralAttractor centralAttractor;

    [Header("Rocket Controls")]
    [SerializeField] private RectTransform keyUpBtn;
    [SerializeField] private RectTransform keyDownBtn;
    [SerializeField] private RectTransform keyLeftBtn;
    [SerializeField] private RectTransform keyRightBtn;
    [SerializeField] private VerticalLayoutGroup keyBtnLayout;

    [Header("Extra")]
    [SerializeField] private GameObject referenceFrame;
    [SerializeField] private LabelPositionManager equationsManager;
    [SerializeField] private InputsRotateObject inputsArrowManager;
    [SerializeField] private RectTransform rocketPanel;
    [SerializeField] private CallEvents callOnSpaceEvent;
    [SerializeField] private CallEvents callOnBackSpaceEvent;

    public override void Awake()
    {
        base.Awake();
        
        currentAffordances = Instantiate(defaultAffordances);
        ResetApp();

        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.captureAllKeyboardInput so elements in web page can handle keyboard inputs
            WebGLInput.captureAllKeyboardInput = false;
        #endif
    }

    public void ResetAppFromJSON(string affordanceJson)
    {
        currentAffordances = Instantiate(defaultAffordances);
        JsonUtility.FromJsonOverwrite(affordanceJson, currentAffordances);
        ResetApp();
    }

    public void ResetApp()
    {
        // Main control config:
        playButton.gameObject.SetActive(currentAffordances.showPlayButton);
        resetButton.gameObject.SetActive(currentAffordances.showResetButton);
        isResetEnable.Value = currentAffordances.showResetButton;
        // Rocket config:
        rocket.transform.SetPositionAndRotation(currentAffordances.physicalObject.initialPosition.ToVector3(), Quaternion.identity);
        rocket.transform.Find("RocketObject").transform.rotation = Quaternion.Euler(currentAffordances.physicalObject.initialRotation.ToVector3());
        rocketVelocity.Value = currentAffordances.physicalObject.initialVelocity.ToVector3();
        rocketVelocityVector.SetActive(currentAffordances.physicalObject.showVelocityVector);
        rocketVelocityVector.GetComponent<DraggableVector>().SetInteractable(currentAffordances.physicalObject.velocityVectorIsInteractive);
        rocketVelocityVector.GetComponent<DraggableVector>().Redraw();
        
        playButton.GetComponent<PlayButton>().PlayWithoutRaising();
        rocket.GetComponent<Rigidbody>().isKinematic = false;
        rocket.GetComponent<Rigidbody>().velocity = rocketVelocity.Value;

        velocityLabel.SetActive(currentAffordances.physicalObject.showVelocityLabel);
        showVelocityEquation.Value = currentAffordances.physicalObject.showVelocityEquation;
        rocketIsInteractiveUp.Value = currentAffordances.physicalObject.isInteractiveUp;
        rocketIsInteractiveDown.Value = currentAffordances.physicalObject.isInteractiveDown;
        rocketIsInteractiveRight.Value = currentAffordances.physicalObject.isInteractiveRight;
        rocketIsInteractiveLeft.Value = currentAffordances.physicalObject.isInteractiveLeft;

        keyUpBtn.gameObject.SetActive(currentAffordances.physicalObject.isInteractiveUp);
        keyDownBtn.gameObject.SetActive(currentAffordances.physicalObject.isInteractiveDown);
        keyLeftBtn.gameObject.SetActive(currentAffordances.physicalObject.isInteractiveLeft);
        keyRightBtn.gameObject.SetActive(currentAffordances.physicalObject.isInteractiveRight);

        if (currentAffordances.physicalObject.isInteractiveLeft || currentAffordances.physicalObject.isInteractiveRight)
        {
            keyLeftBtn.parent.gameObject.SetActive(true);
        }
        else
        {
            keyLeftBtn.parent.gameObject.SetActive(false);
        }

        if (currentAffordances.physicalObject.isInteractiveUp)
        {
            keyBtnLayout.padding.top = 0;
        }
        else
        {
            keyBtnLayout.padding.top = 10;
        }

        if (currentAffordances.physicalObject.isInteractiveDown)
        {
            keyBtnLayout.padding.bottom = 0;
        }
        else
        {
            keyBtnLayout.padding.bottom = 10;
        }

        bool rocketInputsActivation = currentAffordances.physicalObject.isInteractiveUp ||
            currentAffordances.physicalObject.isInteractiveDown ||
            currentAffordances.physicalObject.isInteractiveLeft ||
            currentAffordances.physicalObject.isInteractiveRight;
        keyUpBtn.parent.gameObject.SetActive(rocketInputsActivation);

        // Path Renderer config:
        showRocketPath.Value = currentAffordances.physicalObject.showTrace;
        showRocketPathToggle.gameObject.SetActive(currentAffordances.physicalObject.showTraceIsInteractive);
        showRocketPathToggle.GetComponent<ToggleIcons>().SetWithoutRaising(currentAffordances.physicalObject.showTrace);
        // Thrust Config:
        thrustIsActive.Value = currentAffordances.thrustForce.isActive;
        thrustShowVector.Value = currentAffordances.thrustForce.showVector;

        thrustForce.Value = Vector3.up * currentAffordances.thrustForce.initialMagnitude;
        thrustForce.Value = Quaternion.Euler(currentAffordances.physicalObject.initialRotation.ToVector3()) * thrustForce.Value;

        thrustShowEquation.Value = currentAffordances.thrustForce.showEquation;
        thrustShowLabel.SetActive(currentAffordances.thrustForce.showLabel);
        thrustIsInteractive.Value = currentAffordances.thrustForce.isInteractive;

        // Central force config:
        centralForceIsActive.Value = currentAffordances.centralForceisActive;
        centralForceShowVector.Value = currentAffordances.centralForceshowVector;

        centralRadius.Value = currentAffordances.centralForceRadius;

        centralForceShowEquation.Value = currentAffordances.centralForceshowEquation;
        centralForceShowLabel.SetActive(currentAffordances.centralForceshowLabel);
        centralForceIsInteractive.Value = currentAffordances.centralForceisInteractive;

        centralAttractor.gameObject.SetActive(centralForceIsActive.Value);
        centralForceToggle.SetToggle(centralForceIsActive.Value);

        // Camera:
        Vector3 cameraPos = currentAffordances.camera.position.ToVector3();
        cameraLockingToggle.SetWithoutRaising(currentAffordances.camera.isLockedOnObject);

        Slider zoomSlider = cameraZoomSlider.GetComponent<Slider>();
        float minDistanceToObject = (rocket.transform.localScale.x + rocket.transform.localScale.y + rocket.transform.localScale.z)/3;
        mainCamera.InitCamera(
            rocket.transform,
            cameraPos,
            currentAffordances.camera.isLockedOnObject,
            minDistanceToObject,
            zoomSlider
        );

        mainCamera.transform.localRotation = Quaternion.Euler(currentAffordances.camera.rotation.ToVector3());

        cameraControls.gameObject.SetActive(currentAffordances.camera.showCameraControl);
        
        // Extra:
        referenceFrame.SetActive(currentAffordances.showReferenceFrame);

        equationsManager.Init();

        inputsArrowManager.Start();

        callOnSpaceEvent.Start();
        callOnBackSpaceEvent.Start();

        // UI position
        bool rocketPanelActivation =
            currentAffordances.physicalObject.isInteractiveLeft ||
            currentAffordances.physicalObject.isInteractiveRight ||
            currentAffordances.thrustForce.isInteractive;
        rocketPanel.gameObject.SetActive(rocketPanelActivation);
        
        if (!currentAffordances.showPlayButton && !currentAffordances.showResetButton)
        {
            metaPanel.gameObject.SetActive(false);
            cameraControls.GetComponent<RectTransform>().anchoredPosition = new Vector2(25, -25);
        }
        else
        {
            metaPanel.gameObject.SetActive(true);
            cameraControls.GetComponent<RectTransform>().anchoredPosition = new Vector2(25, -110);
        }
        velocityLabel.GetComponent<VectorLabel>().UpdateSprite();
        thrustShowLabel.GetComponent<VectorLabel>().UpdateSprite();
    }
}

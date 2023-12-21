using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Affordances", menuName="Affordances")]
public class Affordances : ScriptableObject
{
    public bool showPlayButton;
    public bool showPauseButton;
    public bool showResetButton;
    public bool showTimeControl;
    public A_Camera camera;
    public bool showReferenceFrame;
    public A_PhysicalObject physicalObject;
    public A_Force thrustForce;
    public bool showAsteroidButton;
    public float asteroidCollisionForce;
}

[Serializable]
public class A_Camera
{
    public A_Vector3 position;
    public A_Vector3 rotation; // in degrees!
    public bool showCameraControl;
    public bool isLockedOnObject;
}

[Serializable]
public class A_PhysicalObject
{
    public bool isInteractiveUp;
    public bool isInteractiveDown;
    public bool isInteractiveRight;
    public bool isInteractiveLeft;
    public bool showTrace;
    public bool showTraceIsInteractive;
    public A_Vector3 initialPosition;
    public A_Vector3 initialRotation; // in degrees!
    public bool showVelocityVector;
    public A_Vector3 initialVelocity;
    public bool velocityVectorIsInteractive;
    public bool showVelocityLabel;
    public bool showVelocityEquation;
}

[Serializable]
public class A_Force
{
    public bool isActive;
    public bool isInteractive;
    public bool showVector;
    public bool showLabel;
    public bool showEquation;
    public float initialMagnitude;
}

[Serializable]
public class A_Vector3
{
    public float X;
    public float Y;
    public float Z;

    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, Z);
    }
}
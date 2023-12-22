using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CelestialBodyManager : MonoBehaviour
{
    [SerializeField] GameObject celestialBody;
    [SerializeField] Vector3Variable celestialBodyPosition;
    [SerializeField] BoolVariable gravitationalForceActivation;
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform planeCenter;
    [SerializeField] Button centralForceButton;
    [SerializeField] Material SunMat;
    [SerializeField] Material BlackHoleMat;
    [SerializeField] GameObject BlackHoleDistorsion;
    Plane dragPlane;
    bool isDragged;

    void Start()
    {
        dragPlane = new Plane(Vector3.up, planeCenter.position);
        // celestialBody.transform.position = Vector3.zero;
        // celestialBodyPosition.Value = Vector3.zero;
        // celestialBody.SetActive(false);
        isDragged = false;
    }

    public void ToggleCelestialBody()
    {
        gravitationalForceActivation.Value = false;
        if (celestialBody.activeSelf)
        {
            RemoveCelestialBody();
            centralForceButton.interactable = true;
        }
        else
        {
            StartDrag();
            centralForceButton.interactable = false;
        }
    }

    public void EndDrag()
    {
        isDragged = false;
        if (celestialBody.activeSelf)
        {
            gravitationalForceActivation.Value = true;
        }
    }

    public void StartDrag()
    {
        // config var
        dragPlane = new Plane(Vector3.up, planeCenter.position);
        // Move body to position
        SetPosition();
        // Active body
        celestialBody.SetActive(true);
        isDragged = true;        
    }

    public void RemoveCelestialBody()
    {
        celestialBody.SetActive(false);
    }

    private void SetPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            celestialBody.transform.position = hitPoint;
            celestialBodyPosition.Value = hitPoint;
        }
    }

    void LateUpdate()
    {
        OnDrag();
    }

    public void OnDrag()
    {
        if (isDragged)
        {
            SetPosition();
        }
    }

    public void SetMaterial(bool isSun)
    {
        if (isSun)
        {
            celestialBody.GetComponent<MeshRenderer>().material = SunMat;
            BlackHoleDistorsion.SetActive(false);
        }
        else
        {
            celestialBody.GetComponent<MeshRenderer>().material = BlackHoleMat;
            BlackHoleDistorsion.SetActive(true);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class DraggableVector : Vector
{
    public bool interactable = true;
    [SerializeField] private float dragPlaneDistance = 10f;

    [Header("Interaction Zones")]
    [SerializeField] private VectorClickZone tailClickZone;
    [SerializeField] private VectorClickZone headClickZone;

    [Header("Plane restriction")]
    [SerializeField] private bool useCustomPlane;
    [SerializeField] private Vector3 customPlaneCenter;
    [SerializeField] private Vector3 customPlaneNormal;
    [field: SerializeField] public bool VectorIsOnlyScalable {get; set;} = false;
    [SerializeField] private Vector3Reference DragDirectionIfOnlyScalable;

    [Header("Sticky Point")]
    public bool useStickyPoint;
    public Vector3 stickyPoint;
    public float stickyPointRadius = 0.5f;

    [Header("Sticky Directions")]
    public bool useStickyDirections;
    public List<Vector3> stickyDirections;

    [Header("Head Sticky Point")]
    public BoolReference useStickyPointHead;
    public Vector3Reference stickyPointHead;
    public float stickyPointRadiusHead = 0.5f;

    [Header("Magnitude")]
    public bool clampMagnitude;
    public float minMagnitude = 0.2f;
    public float maxMagnitude = 3f;

    [Header("OnDrag Head Redraw Drag Plane")]
    public bool redrawPlaneOnDragHead = false;
    public Transform vectorWorldPos;

    // For interactions
    private Camera mainCamera;
    private Plane dragPlane;

    private bool draggingTail;
    private bool draggingHead;

    // For resetting
    private Vector3 resetPosition;
    private Vector3 resetComponents;

    private void Awake()
    {
        resetPosition = transform.position;
        resetComponents = components.Value;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        Redraw();

        headClickZone.OnZoneMouseDown += HandleZoneMouseDown;
        tailClickZone.OnZoneMouseDown += HandleZoneMouseDown;
        headClickZone.OnZoneMouseUp += HandleZoneMouseUp;
        tailClickZone.OnZoneMouseUp += HandleZoneMouseUp;
    }

    private void OnDisable()
    {
        headClickZone.OnZoneMouseDown -= HandleZoneMouseDown;
        tailClickZone.OnZoneMouseDown -= HandleZoneMouseDown;
        headClickZone.OnZoneMouseUp -= HandleZoneMouseUp;
        tailClickZone.OnZoneMouseUp -= HandleZoneMouseUp;
    }

    public void HandleZoneMouseDown(VectorClickZone clickZone)
    {
        if (!interactable) return;

        if (clickZone == tailClickZone)
        {
            draggingTail = true;
        }
        else if (clickZone == headClickZone)
        {
            if (redrawPlaneOnDragHead)
            {
                Vector3 planeNormal = -mainCamera.transform.forward;
                Vector3 planePosition = vectorWorldPos.position + components.Value;
                DefineDragPlane(planeNormal, planePosition);
            }
            draggingHead = true;
        }
    }

    public void HandleZoneMouseUp(VectorClickZone clickZone)
    {
        if (clickZone == tailClickZone)
        {
            draggingTail = false;
        }
        else if (clickZone == headClickZone)
        {
            draggingHead = false;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;

        // Create a plane at a fixed distance from the camera, perpendicular to the camera's forward direction
        Vector3 planeNormal = -mainCamera.transform.forward;
        Vector3 planePosition = mainCamera.transform.position + dragPlaneDistance * mainCamera.transform.forward;
        
        if (useCustomPlane)
        {
            DefineDragPlane(customPlaneNormal, customPlaneCenter);
        }
        else 
        {
            DefineDragPlane(planeNormal, planePosition);
        }

    }

    public void DefineDragPlane(Vector3 newPlaneNormal, Vector3 newPlanePosition)
    {
        dragPlane = new Plane(newPlaneNormal, newPlanePosition);
    }

    public override void Redraw()
    {
        base.Redraw();

        if (headClickZone) headClickZone.transform.position = transform.position + components.Value;
    }

    private void LateUpdate()
    {
        if (draggingTail || draggingHead)
        {
            // Create a ray from the mouse click position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Initialise the enter variable
            float enter = 0.0f;

            if (dragPlane.Raycast(ray, out enter))
            {
                // Get the point that is clicked
                Vector3 hitPoint = ray.GetPoint(enter);

                if (draggingTail)
                {
                    // Move the vector to the clicked point
                    transform.position = hitPoint;
                    if (useStickyPoint)
                    {
                        if (Vector3.Distance(hitPoint, stickyPoint) <= stickyPointRadius)
                        {
                            transform.position = stickyPoint;
                        }
                    }
                }
                else
                {
                    // Update components
                    Vector3 newComponents = hitPoint - transform.position;


                    if (VectorIsOnlyScalable)
                    {
                        newComponents = Vector3.Project(newComponents, DragDirectionIfOnlyScalable.Value);
                    }

                    if (useStickyPointHead.Value)
                    {
                        if (Vector3.Distance(newComponents, stickyPointHead.Value) <= stickyPointRadiusHead)
                        {
                            newComponents = stickyPointHead.Value;
                        }
                    }

                    // Snap the direction
                    if (useStickyDirections && transform.position == stickyPoint)
                    {
                        foreach (var direction in stickyDirections)
                        {
                            float cosAngle = Vector3.Dot(newComponents, direction);
                            cosAngle /= (newComponents.magnitude * direction.magnitude);
                            if (cosAngle > 0.98f)
                            {
                                newComponents = newComponents.magnitude * direction.normalized;
                                break;
                            }
                        }
                    }

                    if (clampMagnitude)
                    {
                        newComponents = Vector3.ClampMagnitude(newComponents, maxMagnitude);
                        if (newComponents.magnitude < minMagnitude)
                        {
                            newComponents = minMagnitude * newComponents.normalized;
                        }
                    }

                    components.Value = newComponents;
                    Redraw();
                }
            }
        }
    }

    public void SetClickZoneColors()
    {
        Color clickZoneColor = color - new Color(0, 0, 0, 0.8f);
        if (tailClickZone) tailClickZone.SetColor(clickZoneColor);
        if (headClickZone) headClickZone.SetColor(clickZoneColor);
    }

    public void MakeInteractable()
    {
        SetInteractable(true);
    }

    public void SetInteractable(BoolVariable boolVariable)
    {
        SetInteractable(boolVariable.Value);
    }

    public void SetInteractable(bool value)
    {
        interactable = value;

        if (headClickZone)
        {
            headClickZone.gameObject.SetActive(interactable);
            headClickZone.interactable = interactable;
        }

        if (tailClickZone)
        {
            tailClickZone.gameObject.SetActive(interactable);
            tailClickZone.interactable = interactable;
        }
    }

    public void Reset()
    {
        transform.position = resetPosition;
        components.Value = resetComponents;
        Redraw();

        HideHeadClickZone();
        HideTailClickZone();
    }

    public void ShowTailClickZone(bool interactable)
    {
        if (tailClickZone)
        {
            tailClickZone.gameObject.SetActive(true);
            tailClickZone.interactable = interactable;
            tailClickZone.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void HideTailClickZone()
    {
        if (tailClickZone)
        {
            tailClickZone.gameObject.SetActive(false);
            tailClickZone.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void ShowHeadClickZone(bool interactable)
    {
        if (headClickZone)
        {
            headClickZone.gameObject.SetActive(true);
            headClickZone.interactable = interactable;
            headClickZone.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void HideHeadClickZone()
    {
        if (headClickZone)
        {
            headClickZone.gameObject.SetActive(false);
            headClickZone.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public bool IsDragged()
    {
        return draggingHead | draggingTail;
    }

    public VectorClickZone GetHeadClickZone()
    {
        return headClickZone;
    }

    public VectorClickZone GetTailClickZone()
    {
        return tailClickZone;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    [SerializeField] private BoolReference showPath;
    [SerializeField] private Material materialTrail;
    [SerializeField] private GameObject startTrailIndicator;
    [SerializeField] private BoolVariable isThurstActive;
    [SerializeField] private GameObject startThrustIndicator;
    [SerializeField] private GameObject endThrustIndicator;
    private TrailRenderer trailRenderer;
    private GameObject startPoint;
    private bool isStartPointSet = false;
    private List<GameObject> startThrustPoints;
    private List<GameObject> endThrustPoints;
    private bool isStartThrustIndicatorSet = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!TryGetComponent<TrailRenderer>(out trailRenderer))
        {
            trailRenderer = gameObject.AddComponent<TrailRenderer>();
        }

        trailRenderer.Clear();

        trailRenderer.material = materialTrail;

        // Trail Renderer On:
        trailRenderer.autodestruct = false;
        trailRenderer.enabled = showPath.Value;
        trailRenderer.time = float.PositiveInfinity;
        //trailRenderer.time = 5f;
        trailRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        AnimationCurve curve = new AnimationCurve();
        float curveWidth = 0.1f;
        curve.AddKey(0f, curveWidth);
        curve.AddKey(1f, curveWidth);
        trailRenderer.widthCurve = curve;

        trailRenderer.sortingOrder = 1;

        isStartThrustIndicatorSet = false;
        startThrustPoints = new List<GameObject>();
        endThrustPoints = new List<GameObject>();
    }

    void Update()
    {
       if (!isStartPointSet && trailRenderer)
        {
            if (trailRenderer.positionCount == 0)
            {
                return;
            }

            Vector3 startPosition = trailRenderer.GetPosition(trailRenderer.positionCount-1);
            startPoint = Instantiate(startTrailIndicator);
            startPoint.transform.localPosition = startPosition;
            isStartPointSet = true;

            return;
        } 

        if ((!isStartThrustIndicatorSet) && isThurstActive.Value)
        {
            GameObject startThurstPoint = Instantiate(startThrustIndicator);
            startThurstPoint.transform.localPosition = transform.localPosition;
            startThrustPoints.Add(startThurstPoint);

            isStartThrustIndicatorSet = true;
        }

        if (isStartThrustIndicatorSet && (!isThurstActive.Value))
        {
            GameObject endThurstPoint = Instantiate(endThrustIndicator);
            endThurstPoint.transform.localPosition = transform.localPosition;
            endThrustPoints.Add(endThurstPoint);
            isStartThrustIndicatorSet = false;
        }
    }

    public void OnUpdateShowPath()
    {
        SetPathRenderer(showPath.Value);
    }

    public void SetPathRenderer(bool enable)
    {
        ClearPath();
        if (trailRenderer)
        {
            trailRenderer.enabled = enable;
        }
    }

    public void ClearPath()
    {
        if (trailRenderer)
        {
            trailRenderer.Clear();
        }
        if (startPoint)
        {
            Destroy(startPoint);
            isStartPointSet = false;
        }

        if (startThrustPoints!=null)
        {
            DestroyGameObjectList(startThrustPoints);
        }
        if (endThrustPoints!=null)
        {
            DestroyGameObjectList(endThrustPoints);
        }
    }

    private void DestroyGameObjectList(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            Destroy(gameObjects[i]);
        }
        gameObjects.Clear();
    }
}
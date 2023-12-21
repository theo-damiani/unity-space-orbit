using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private RopeData ropeData;
    private List<GameObject> listOfRopeSegment;
    private GameObject anchor;

    void Awake()
    {
        ropeData.RopeUseGravity = false;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        RopeData.OnRopeLengthUpdate += DefineRopeFromLength;
    }

    void OnDisable()
    {
        RopeData.OnRopeLengthUpdate -= DefineRopeFromLength;
    }

    public void BuildRopeTowards(GameObject target)
    {
        SetRopeDirectionTowards(target);
        // DefineRopeFromLength();
    }

    public void SetRopeLocationAt(GameObject newLocation)
    {
        transform.localPosition = newLocation.transform.localPosition;
    }

    public void SetRopeDirectionTowards(GameObject target)
    {        
        transform.rotation = Quaternion.identity;
        transform.Rotate(
            Quaternion.LookRotation(target.transform.localPosition-transform.localPosition).eulerAngles
        );
        //rope.transform.LookAt(ball.transform);
        ropeData.RopeLength = (transform.localPosition - target.transform.localPosition).magnitude;
    }

    private void InitializeAnchor()
    {
        if (anchor==null)
        {
            anchor = new GameObject("Rope Anchor");
            anchor.AddComponent<Rigidbody>();
            anchor.GetComponent<Rigidbody>().isKinematic = true;
            anchor.transform.SetParent(transform, false); // false because we do not want to keep the same parent-child offset relation in position.
            // then with false, anchor.transform is not changed, therefore it stays at 0,0,0.
        }
    }

    public void DefineRopeFromLength()
    {
        InitializeAnchor();

        if (listOfRopeSegment is not null) 
        {
            // Destroy previous rope
            for (int i = 0; i < listOfRopeSegment.Count; i++)
            {
                Destroy(listOfRopeSegment[i]);
            }
            listOfRopeSegment.Clear();
        }


        // Size of each segment in the direction of the rope 
        float sizeSegment = Vector3.Scale(ropeData.RopeSegmentScale, transform.forward).magnitude;
        int numberOfRopeSegment = (int) Mathf.Ceil(ropeData.RopeLength/sizeSegment);
        listOfRopeSegment = new List<GameObject>();

        // Instantiate rope Segments 
        for (int i = 0; i < numberOfRopeSegment; i++)
        {
            GameObject segment = Instantiate(ropeData.ropeSegmentPrefab, anchor.transform);

            listOfRopeSegment.Add(segment);

            //Configure transform component of segment
            segment.transform.localScale = ropeData.RopeSegmentScale;
            segment.transform.localRotation = Quaternion.identity;

            // Configure Joint between each segment
            ConfigurableJoint joint = segment.GetComponent<ConfigurableJoint>();
            joint.anchor = Vector3.zero; // relative position of the anchor in the segment reference system. Set to the center of the segment.


            Vector3 offsetAlongForward = transform.InverseTransformVector(Vector3.Scale(transform.forward, ropeData.RopeSegmentScale));
            if (i==0)
            {
                // first segment to initialize.
                segment.transform.localPosition = anchor.transform.localPosition + offsetAlongForward;
                joint.connectedBody = anchor.GetComponent<Rigidbody>();
            }
            else
            {
                segment.transform.localPosition = listOfRopeSegment[i-1].transform.localPosition + offsetAlongForward;
                //joint.anchor = -ropeDirection.normalized/2;
                joint.connectedBody = listOfRopeSegment[i-1].GetComponent<Rigidbody>();
                //joint.autoConfigureConnectedAnchor = false;
                //joint.connectedAnchor = ropeDirection.normalized/2;
            }
        }

        SetJointsFromRopeGravity();
    }

    public void ReleaseRope()
    {
        if (gameObject.activeSelf)
        {
            ropeData.RopeUseGravity = true;
            SetJointsFromRopeGravity();
        }
    }

    public void SetUseGravity(bool value)
    {
        ropeData.RopeUseGravity = value;
        SetJointsFromRopeGravity();
    }

    private void SetJointsFromRopeGravity()
    {
        for (int i = 0; i < listOfRopeSegment.Count; i++)
        {
            // apply gravity to every segment except one at origin (i>0),
            // Segment at origin remains fixed.
            GameObject segment = listOfRopeSegment[i];

            segment.GetComponent<Rigidbody>().useGravity = ropeData.RopeUseGravity;

            ConfigurableJoint joint = segment.GetComponent<ConfigurableJoint>();

            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;

            if (ropeData.RopeUseGravity)
            {
                joint.angularXMotion = ConfigurableJointMotion.Free;
                joint.angularYMotion = ConfigurableJointMotion.Locked;
                joint.angularZMotion = ConfigurableJointMotion.Locked;
            }
            else 
            {
                joint.angularXMotion = ConfigurableJointMotion.Locked;
                joint.angularYMotion = ConfigurableJointMotion.Locked;
                joint.angularZMotion = ConfigurableJointMotion.Locked;
            }
            
        }
    }
}

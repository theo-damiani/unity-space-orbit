using System;
using UnityEngine;

[Serializable]
public class MotionData
{
    public Motion motion;
    public bool isInit;
    public BoolReference isActive;
}

[RequireComponent(typeof(Rigidbody))]
public class MotionManager : MonoBehaviour
{
    [SerializeField] private MotionData[] listMotionData;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // rb.velocity = Vector3.zero;
        for (int i = 0; i < listMotionData.Length; i++)
        {
            if (listMotionData[i].isActive.Value)
            {
                if (!listMotionData[i].isInit)
                {
                    listMotionData[i].motion.InitMotion(rb);
                    listMotionData[i].isInit = true;
                }
                listMotionData[i].motion.ApplyMotion(rb);
            }
            else
            {
                listMotionData[i].isInit = false;
            }
        }
    }

    public void SetMotionIndex(int index)
    {
        if (index >= listMotionData.Length)
        {return;}

        for (int i = 0; i < listMotionData.Length; i++)
        {
            if (i==index)
            {
                listMotionData[i].isInit = false;
                listMotionData[i].isActive.Value = true;
            }
            else
            {
                listMotionData[i].isActive.Value = false;
            }
        }
    }

    public void StopMotionIndex(int index)
    {
        if (index >= listMotionData.Length)
        {return;}

        listMotionData[index].isActive.Value = false;
    }

    public void StopAllMotion()
    {
        for (int i = 0; i < listMotionData.Length; i++)
        {
            listMotionData[i].isActive.Value = false;
        }
    }
}

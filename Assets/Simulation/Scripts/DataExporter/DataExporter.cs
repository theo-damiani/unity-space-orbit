using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine; 

[Serializable]
public struct DataHolder
{
    public DataHolder(float t, Vector3 p, Vector3 r, Vector3 v)
    {
        time = t;
        position = p;
        rotation = r;
        velocity = v;
    }
    public float time;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 velocity;
}

public class DataExporter : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GameObjectDataRecordingDone (string dataJSON);

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float timeRate;
    [SerializeField] private int maxSize;
    [SerializeField] private int indexAxisForX = 0;
    [SerializeField] private int indexAxisForY = 1;
    [SerializeField] private int indexAxisForZ = 2;
    [SerializeField] private Material outlineMat;

    private readonly float defaultTimeRate = 0.1f;
    private List<string> listOfJsonData;
    private float timeAtStart;
    private bool isRecording;

    void Start()
    {
        listOfJsonData = new List<string>();

        // From InvokeRepeating: throw new UnityException("Invoke repeat rate has to be larger than 0.00001F)");
        SetTimeRate(timeRate);
        isRecording = false;
        SetOutlineThickness(0f);
    }

    public void SetTimeRate(float newRate)
    {
        timeRate = (newRate < 0.00001F) ? defaultTimeRate : newRate;

        if (isRecording)
        {
            StopGameObjectRecording();
        }
    }

    public void StartGameObjectRecording()
    {
        isRecording = true;
        timeAtStart = Time.realtimeSinceStartup;
        listOfJsonData.Clear();
        InvokeRepeating(nameof(SaveData), 0f, timeRate);
        SetOutlineThickness(1.1f);
    }

    public void StopGameObjectRecording()
    {
        isRecording = false;
        CancelInvoke(nameof(SaveData));

        #if UNITY_WEBGL == true && UNITY_EDITOR == false
            GameObjectDataRecordingDone(JsonUtility.ToJson(new JsonableListWrapper<string>(listOfJsonData)));
        #endif

        listOfJsonData.Clear();
        SetOutlineThickness(0f);
    }

    void SaveData()
    {
        if (listOfJsonData.Count <= maxSize)
        {
            DataHolder data = new(
                    Time.realtimeSinceStartup - timeAtStart,
                    SwitchComponentsAxis(_rigidbody.transform.transform.position),
                    SwitchComponentsAxis(_rigidbody.transform.rotation.eulerAngles),
                    SwitchComponentsAxis(_rigidbody.velocity)
                );

            listOfJsonData.Add(JsonUtility.ToJson(data));
        }
        else
        {
            StopGameObjectRecording();
        }
    }

    private void SetOutlineThickness(float thickness)
    {
        if (outlineMat)
        {
            outlineMat.SetFloat("_Thickness", thickness);
        }
    }

    private Vector3 SwitchComponentsAxis(Vector3 components)
    {
        return new Vector3(components[indexAxisForX], components[indexAxisForY], components[indexAxisForZ]);
    }
}

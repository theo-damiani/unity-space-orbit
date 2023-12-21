using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public enum UnityActionType {
    Click,
    Drag
}

// Unity do not support the serialization of nested abstract object:
/*
[Serializable]
public class MainType
{
    public string p1;
    public List<ParentType> p2;
} 

[Serializable]
public abstract class ParentType {}

[Serializable]
public class SubTypeA : ParentType
{
    public int a;
} 

[Serializable]
public class SubTypeB : ParentType
{
    public string b;
} 

Unity will not serialize the property p2 in the MainType !
That is why ParentType has been removed, the MainType has been made abstract and the definition has been moved subclasses.
*/

[Serializable]
public abstract class UserTraceHolder {}

public abstract class AnalyticsExporter : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void NewUnityUserTrace (string dataJSON);
    public void SendNewTrace(UserTraceHolder trace)
    {
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
            NewUnityUserTrace(JsonUtility.ToJson(trace));
        #endif
    }    
}

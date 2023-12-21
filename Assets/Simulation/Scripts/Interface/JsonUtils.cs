using System;
using System.Collections.Generic;

[Serializable]
public class JsonableListWrapper<T>
{
    public List<T> list;
    public JsonableListWrapper(List<T> list) => this.list = list;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseVariable<T> : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    [SerializeField]
    private T _value = default(T);
    [SerializeField]
    private bool _readOnly = false;
    [SerializeField]
    private bool _useDefaultValue = false;
    [SerializeField]
    private bool _raiseWarning = true;
    [SerializeField]
    private bool _isClamped = false;
    [SerializeField]
    private T _minClampedValue = default(T);
    [SerializeField]
    private T _maxClampedValue = default(T);
    [SerializeField]
    private T _defaultValue;
    [SerializeField]
    public GameEvent OnUpdateEvent;

    public virtual T Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = SetValue(value);
        }
    }

    public T DefaultValue
    {
        get => _defaultValue;
        set => _defaultValue = value;
    }

    public virtual T MinClampValue
    {
        get
        {
            if(Clampable)
            {
                return _minClampedValue;
            }
            else
            {
                return default(T);
            }
        }
    }

    public virtual T MaxClampValue
    {
        get
        {
            if(Clampable)
            {
                return _maxClampedValue;
            }
            else
            {
                return default(T);
            }
        }
    }

    public virtual bool Clampable { get { return false; } }
    public virtual bool ReadOnly { get { return _readOnly; } }
    public bool IsClamped { get { return _isClamped; } }
    public System.Type Type { get { return typeof(T); } }
    public bool UseDefaultValue => _useDefaultValue;

    public virtual T SetValue(BaseVariable<T> value)
    {
        return SetValue(value.Value);
    }
    public virtual T SetValue(T newValue)
    {
        if (ReadOnly)
        {
            RaiseReadonlyWarning();
            return _value;
        }
        else if(Clampable && IsClamped)
        {
            newValue = ClampValue(newValue);
        }

        _value = newValue;
        if (OnUpdateEvent)
                    OnUpdateEvent.Raise();

        return newValue;
    }
    protected virtual T ClampValue(T value)
    {
        return value;
    }
    private void RaiseReadonlyWarning()
    {
        if (!ReadOnly || !_raiseWarning)
            return;

        Debug.LogWarning("Tried to set value on " + this + ", but value is readonly!");
    }
    public override string ToString()
    {
        return _value == null ? "null" : _value.ToString();
    }
    public void OnValidate()
    {
        SetValue(Value);
    }
    public void OnEnable()
    {
        if(UseDefaultValue)
            ResetToDefaultValue();
    }
    private void ResetToDefaultValue()
    {
        Value = _defaultValue;
    }
}
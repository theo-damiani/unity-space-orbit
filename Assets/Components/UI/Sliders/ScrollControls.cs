using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ScrollControls : MonoBehaviour
{
    private Slider slider;
    private float scale = 0.1f;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        float mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll!=0)
        {
            slider.value += mouseScroll*scale;
        }
    }
}

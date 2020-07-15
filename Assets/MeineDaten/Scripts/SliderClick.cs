using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderClick : MonoBehaviour
{
    public Slider slider;
    public AudioSource clicksound;
    private float startValue;
    private float endValue;
    private float lastSliderValue;
    private bool dragStarted = false;
    private bool dragEnded = false;
    private bool clickNextToHandle = false;
    private SliderControl sliderControl;
    private ValueControlCenter valueControlCenter;
    

    // Start is called before the first frame update
    void Start()
    {
        sliderControl = GameObject.Find("SliderControl").GetComponent<SliderControl>();
        valueControlCenter = GameObject.Find("SliderControl").GetComponent<ValueControlCenter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (valueControlCenter.touchscreenInput)
        {
            if (dragStarted && dragEnded)
            {
                if (startValue == endValue)
                {
                    sliderControl.Comparision();
                    clicksound.Play();
                }
                dragEnded = dragStarted = false;
            }
        }
        lastSliderValue = slider.value;
    }

    public void TouchBegins() //attach this to OnPointerDown
    {
        startValue = slider.value;
        if (startValue == lastSliderValue) //else it's a click next to the handle
        {
            dragStarted = true;
        }
        else
        {
            clickNextToHandle = true;
        }
    }

    public void TouchEnds() //attach this to OnPointerUp
    {
        if (!clickNextToHandle)
        {
            endValue = slider.value;
            dragEnded = true;
        }
        else
        {
            clickNextToHandle = false;
        }
    }
}

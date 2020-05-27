using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScrollAufgabe : MonoBehaviour
{
    [Header("Scroll/Drag Sensitivity")]
    public float sensitivityOneFinger; //Start value = 2f
    public float sensitivityTwoFinger; //Start value = 0.08f

    private ValueControlCenter valueControlCenter;

    private void Awake()
    {
        valueControlCenter = GameObject.Find("ScrollManager").GetComponent<ValueControlCenter>();
    }

    private void Start()
    {
        EnableScrollRectMovement();
    }

    public void EnableScrollRectMovement()
    {
        if (valueControlCenter.touchscreenInput == false)
        {
            GameObject.Find("ButtonScrollList").GetComponent<ScrollRectMovement>().enabled = true;
        }
        else
        {
            GameObject.Find("ButtonScrollList").GetComponent<ScrollRectMovement>().enabled = false;
        }
    }
}

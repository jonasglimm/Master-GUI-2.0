﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ValueManipulation : MonoBehaviour
{
    [Header("GUI Elements")]
    public Image selector;
    public TextMeshProUGUI selectionValue;
    
    [Header("Sounds")]
    public AudioSource scrollSound;
    private int value;
    private ControlManager script;
    private TextMeshProUGUI maxValueText;

    private int maxValue;
    private float minFillAmountDisplayed = 0.11f; // Because of ackward rotation of the image a part of the image is not visible
    private float maxFillAmountDisplayed = 0.95f; // Because of ackward rotation of the image a part of the image is not visible
    private float usableFillAmount;
    private float fillAmountStep;

    private void Awake()
    {
        maxValueText = GameObject.Find("MaxValue").GetComponent<TextMeshProUGUI>();
        script = gameObject.GetComponent<ControlManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxValue = script.maxValue;
        usableFillAmount = maxFillAmountDisplayed - minFillAmountDisplayed;
        fillAmountStep = usableFillAmount / maxValue;

        value = maxValue / 3;
        selector.fillAmount = fillAmountStep * (float)value + minFillAmountDisplayed;
        selectionValue.text = value.ToString();
        maxValueText.text = maxValue.ToString();
    }

    void Update()
    {
        if (script.iDriveInput == true)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                if (value < maxValue)
                {
                    selector.fillAmount = selector.fillAmount + fillAmountStep;
                        scrollSound.Play();
                        value++;
                        selectionValue.text = value.ToString();
                }
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                if (value > 0)
                {
                    selector.fillAmount = selector.fillAmount - fillAmountStep;
                        scrollSound.Play();
                        value--;
                        selectionValue.text = value.ToString();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                script.checkOutput(selectionValue.text);
            }
        }
    }

}

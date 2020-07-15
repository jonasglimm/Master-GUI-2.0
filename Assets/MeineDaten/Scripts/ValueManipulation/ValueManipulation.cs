using System.Collections;
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
    private IDriveController iDriveController;

    private void Awake()
    {
        maxValueText = GameObject.Find("MaxValue").GetComponent<TextMeshProUGUI>();
        script = gameObject.GetComponent<ControlManager>();
        iDriveController = GameObject.Find("Manager").GetComponent<IDriveController>();
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

        if (!script.iDriveInput)
        {
            iDriveController.enabled = false;
        }
    }

    void Update()
    {
        if (script.iDriveInput == true) //script.iDriveInput == true  - for control with mouse wheel
        {
            if (iDriveController.turnedClockwise)
            {
                if (value < maxValue)
                {
                    if (value + iDriveController.rotationClockwiseSteps <= maxValue)
                    {
                        selector.fillAmount = selector.fillAmount + (iDriveController.rotationClockwiseSteps * fillAmountStep);
                        value = value + iDriveController.rotationClockwiseSteps;
                    }
                    else
                    {
                        selector.fillAmount = maxFillAmountDisplayed;
                        value = maxValue;
                    }
                    scrollSound.Play();
                    selectionValue.text = value.ToString();
                }
            }

            if (iDriveController.turnedCounterclockwise)
            {
                if (value > 0)
                {
                    if (value - iDriveController.rotationCounterclockwiseSteps >= 0)
                    {
                        selector.fillAmount = selector.fillAmount - (iDriveController.rotationCounterclockwiseSteps * fillAmountStep);
                        value = value - iDriveController.rotationCounterclockwiseSteps;
                    }
                    else
                    {
                        selector.fillAmount = minFillAmountDisplayed;
                        value = 0;
                    }
                    scrollSound.Play();
                    selectionValue.text = value.ToString();
                }
            }

            if (iDriveController.pushedOnce) //Input.GetMouseButtonDown(0) - for mouse input
            {
                script.checkOutput(selectionValue.text);
            }
        }
    }

}

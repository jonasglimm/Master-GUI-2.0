using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ScrollRectMovement : MonoBehaviour
// not using Start() because array of Buttons needs to be intiated before this SetUp() calls.
//Gets called from ButtonListControlMitTasten - Script
{
    public ScrollRect scrollRect;
    public int startButton = 0; //Start value = 0
    public float lerpTime = 0.1f; //Start value = 0.1f

    private Button[] buttons;
    private int numberOfButtons;
    private int index;
    private float verticalPosition;
    public Button selectedButton;
    public TextMeshProUGUI[] buttonText;
    private Vector3 lastMouseCoordinate = Vector3.zero;

    private float twoFingerScrollMovement; //Start value = 0.08f
    private float oneFingerScrollMovement; //Start value = 2f

    private ValueControlCenter valueControlCenter;
    private StartScrollAufgabe startScrollAufgabe;
    private IDriveController iDriveController;

    public AudioSource scrollingSound;

    private GameObject normalScrollbar;
    private GameObject normalSlidingArea;
    private GameObject iDriveScrollbar;
    private GameObject roundHandle;



    private void Awake()
    {
        valueControlCenter = GameObject.Find("ScrollManager").GetComponent<ValueControlCenter>();
        startScrollAufgabe = GameObject.Find("ScrollManager").GetComponent<StartScrollAufgabe>();
        normalScrollbar = GameObject.Find("ButtonListScrollbar");
        normalSlidingArea = GameObject.Find("Sliding Area");
        iDriveScrollbar = GameObject.Find("RoundScrollbar");
        roundHandle = GameObject.Find("RoundHandle");
        iDriveController = GameObject.Find("ScrollManager").GetComponent<IDriveController>();

        oneFingerScrollMovement = startScrollAufgabe.sensitivityOneFinger;
        twoFingerScrollMovement = startScrollAufgabe.sensitivityTwoFinger;
    }

    public void SetUp()
    {
        buttons = GetComponentsInChildren<Button>();
        numberOfButtons = buttons.Length;
        index = startButton;
        buttons[index].Select();
        verticalPosition = 1f - ((float)index / (buttons.Length - 1));

        if (valueControlCenter.touchpadInput == true)
        {
            InvokeRepeating("CursorLock", valueControlCenter.cursorResetTime, valueControlCenter.cursorResetTime);  // If the trackpad is used, the cursor will be reset to the middle of the screen each cursorResetTime - seconds
            Cursor.visible = false;
        }

        if (valueControlCenter.iDriveInput)
        {
            normalScrollbar.GetComponent<Image>().enabled = false;
            normalSlidingArea.SetActive(false);
        }
        else
        {
            iDriveScrollbar.SetActive(false);
        }
    }

    void Update()
    {
        if(valueControlCenter.touchpadInput == true){
            CursorUnlock();
            handleTwoFingerScroll();
            handleOneFingerScroll();
            selectedButton = buttons[index];
            selectedButton.Select();
            buttonText = selectedButton.GetComponentsInChildren<TextMeshProUGUI>();
        } else if(valueControlCenter.iDriveInput == true)
        {
            //selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            //index = System.Array.IndexOf(buttons, selectedButton);
            iDriveScrolling();
            IDriveScrollbar();
            selectedButton = buttons[index];
            selectedButton.Select();
            buttonText = selectedButton.GetComponentsInChildren<TextMeshProUGUI>();
        }
        verticalPosition = 1f - ((float)index / (buttons.Length - 1));
        scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, verticalPosition, Time.deltaTime / lerpTime);
    }
   

    void handleOneFingerScroll(){
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        if(mouseDelta.y > oneFingerScrollMovement){ // if difference less than zero, moved to left
            lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
            if(index < numberOfButtons-1){
                index++;
                scrollingSound.Play();
            }
        } else if(mouseDelta.y < -oneFingerScrollMovement ){ // if difference greater than zero, moved to right
            lastMouseCoordinate = Input.mousePosition;
            if(index > 0){
                index--;
                scrollingSound.Play();
            }
        } 
    }
    void handleTwoFingerScroll(){
        if(Input.mouseScrollDelta.y < -twoFingerScrollMovement){
            if(index < numberOfButtons-1){
                index++;
                scrollingSound.Play();
            }
        } else if(Input.mouseScrollDelta.y > twoFingerScrollMovement){
            if(index > 0){
                index--;
                scrollingSound.Play();
            }
        }
    }

    private void CursorLock() //reset the Cursor by first locking it with this function and unlock it with the next on
    {
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void CursorUnlock()
    {
        if (Cursor.lockState == CursorLockMode.Locked) // If Cursor is Locked, unlock it to reset in the middle of the screen
        {
            Cursor.lockState = CursorLockMode.None;
            lastMouseCoordinate = Input.mousePosition; //prevent the false recognition of cursor reset as a swipe
        }
    }

    private void iDriveScrolling()
    {
        if (valueControlCenter.iDriveInput == true)
        {
            /* // Using the keyboard
            if (Input.GetKeyDown(KeyCode.DownArrow) | Input.GetKeyDown(KeyCode.UpArrow))
            {
                scrollingSound.Play();
            }*/

            if (iDriveController.rotationClockwiseSteps > 0)
            {
                if (index <= numberOfButtons - 1)
                {
                    if (index + iDriveController.rotationClockwiseSteps < numberOfButtons - 1)
                    {
                        index = index + iDriveController.rotationClockwiseSteps;
                    }
                    else
                    {
                        index = numberOfButtons - 1;
                    }
                    scrollingSound.Play();
                }
            }
            else if (iDriveController.rotationCounterclockwiseSteps > 0)
            {
                if (index > 0)
                {
                    if (index - iDriveController.rotationCounterclockwiseSteps >= 0)
                    {
                        index = index - iDriveController.rotationCounterclockwiseSteps;
                    }
                    else
                    {
                        index = 0;
                    }
                    scrollingSound.Play();
                }
            }
        }
    }

    private void IDriveScrollbar()
    {
        var handleRotation = roundHandle.GetComponent<Transform>().rotation.eulerAngles;
        //topRotation is 24.25 degrees
        //buttonRotatin is 335.75 degrees
        //rotation within = 48.5 degrees
        
        float rotationStep = 48.5f / (float)(numberOfButtons - 1);

        if (iDriveController.turnedCounterclockwise && (handleRotation.z < 24.25 || handleRotation.z >= 330))
        {
            handleRotation.z = handleRotation.z + rotationStep * iDriveController.rotationCounterclockwiseSteps;
        }
        else if (iDriveController.turnedClockwise && (handleRotation.z <= 30 || handleRotation.z > 335.75))
        {
            handleRotation.z = handleRotation.z - rotationStep * iDriveController.rotationClockwiseSteps;
        }
        //Debug.Log(handleRotation);
        
        roundHandle.GetComponent<Transform>().rotation = Quaternion.Euler(handleRotation);
    }

}

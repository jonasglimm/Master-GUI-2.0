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

    public AudioSource scrollingSound;

    private void Awake()
    {
        valueControlCenter = GameObject.Find("ScrollManager").GetComponent<ValueControlCenter>();
        startScrollAufgabe = GameObject.Find("ScrollManager").GetComponent<StartScrollAufgabe>();
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
            selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            index = System.Array.IndexOf(buttons, selectedButton);
            iDriveScrollingSound();
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

    private void iDriveScrollingSound()
    {
        if (valueControlCenter.iDriveInput == true)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) | Input.GetKeyDown(KeyCode.UpArrow))
            {
                scrollingSound.Play();
            }
        }
    }
}

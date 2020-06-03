using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class AuswahlTrackpad : MonoBehaviour{
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    private Button currentButton;
    private Vector3 lastMouseCoordinate = Vector3.zero; // used to store the last mose moved co-ordinates. Initialized with (0,0,0)
    private bool swipeInProgress = false;

    private bool isTrackpadEnabled;
    private bool touchscreenInput;
    private bool iDriveInput;
    private bool gestureInput;

    private float cursorResetTime;

    public ValueControlCenter valueControlCenter; 
    public AudioSource clickSound; // for a click (only used for TouchpadInput - for Touch and iDrive it is played via onClick() of the button
    public AudioSource scrollingSound;

    void Awake()
    {
        isTrackpadEnabled = valueControlCenter.touchpadInput;
        touchscreenInput = valueControlCenter.touchscreenInput;
        iDriveInput = valueControlCenter.iDriveInput;
        gestureInput = valueControlCenter.gestureInput;

        cursorResetTime = valueControlCenter.cursorResetTime;
    }

    void Start()
    {
        if (touchscreenInput == false)
        {
            SelectButton(valueControlCenter.startButton);

            if (isTrackpadEnabled == true)
            {
                InvokeRepeating("CursorLock", cursorResetTime, cursorResetTime);  // If the trackpad is used, the cursor will be reset to the middle of the screen each cursorResetTime - seconds
                HideCursor();
            }
        }
    }

    void SelectButton(Button btn)
    {
        btn.Select(); 
        currentButton = btn;
    }
    void moveLeft(){
        if(currentButton == button2){
            StartCoroutine(waiter(button1));
            // SelectButton(button1);
        } else if(currentButton == button3){
            StartCoroutine(waiter(button2));
            // SelectButton(button2);
        } else if(currentButton == button5){
            StartCoroutine(waiter(button4));
            // SelectButton(button4);
        } else if(currentButton == button6){
            StartCoroutine(waiter(button5));
            // SelectButton(button5);
        }
        scrollingSound.Play();
    }

    void moveRight(){
        if(currentButton == button1){
            StartCoroutine(waiter(button2));
            // SelectButton(button2);
        } else if(currentButton == button2){
            StartCoroutine(waiter(button3));
            // SelectButton(button3);
        } else if(currentButton == button4){
            StartCoroutine(waiter(button5));
            // SelectButton(button5);
        } else if(currentButton == button5){
            StartCoroutine(waiter(button6));
            // SelectButton(button6);
        }
        scrollingSound.Play();
    }

    void moveUp(){
        if(currentButton == button4){
            StartCoroutine(waiter(button1));
            // SelectButton(button1);
        } else if(currentButton == button5){
            StartCoroutine(waiter(button2));
            // SelectButton(button2);
        } else if(currentButton == button6){
            StartCoroutine(waiter(button3));
            // SelectButton(button3);
        }
        scrollingSound.Play();
    }

    void moveDown(){
        if(currentButton == button1){
            StartCoroutine(waiter(button4));
            // SelectButton(button4);
        } else if(currentButton == button2){
            StartCoroutine(waiter(button5));
            // SelectButton(button5);
        } else if(currentButton == button3){
            StartCoroutine(waiter(button6));
            // SelectButton(button6);
        }
        scrollingSound.Play();
    }
    
    
    IEnumerator waiter(Button btn){
        yield return new WaitForSeconds(0.115f);
        SelectButton(btn);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (isTrackpadEnabled == true){
            CursorUnlock(); // Unlock and reset Cursor if it is locked
            //handling trackpad swipe as input
            handleTrackpadGesture();
        } else if(iDriveInput == true){
            //handling keyboard arrrow keys as input
            handleKeyboardInput();
        }


    }
    void handleTrackpadGesture(){
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        //value 20 works for normal move ment without gesture for both directions
        float mouseDeltaX = mouseDelta.x;
        float mouseDeltaY = mouseDelta.y;
        
        if(Input.GetMouseButtonDown(0)){
            AuswahlControl script = gameObject.GetComponent<AuswahlControl>();
            script.Comparision(currentButton);
            clickSound.Play();
            SelectButton(currentButton);
        }
        if(mouseDeltaY < 0){
            mouseDeltaY = -mouseDeltaY;
        }
        if(mouseDeltaX < 0){
            mouseDeltaX = -mouseDeltaX;
        }
        if((mouseDeltaX) > mouseDeltaY){
            // Debug.Log("mouseDeltaX "+mouseDeltaX.ToString());
        // Debug.Log("mouseDeltaY "+mouseDeltaY.ToString());
        // prev value -15
            if(mouseDelta.x < -8){ // if difference less than zero, moved to left
                lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
                if(swipeInProgress == false){ // checking if the swipe gesture that had been started is still in progress or not. 
                    moveLeft();
                    swipeInProgress = true; // swipe gesture is taking place
                }
                //prev value 15
            } else if(mouseDelta.x > 8){ // if difference greater than zero, moved to right
                lastMouseCoordinate = Input.mousePosition;
                if(swipeInProgress == false){
                    moveRight();
                    swipeInProgress = true;
                }
            }
        } else if((mouseDeltaX) < mouseDeltaY) {
             if(mouseDelta.y < -5){ // if difference less than zero, moved down
                lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
                if(swipeInProgress == false){ // checking if the swipe gesture that had been started is still in progress or not. 
                    moveDown();
                    swipeInProgress = true; // swipe gesture is taking place
                }
            } else if(mouseDelta.y > 5){ // if difference greater than zero, moved up
                lastMouseCoordinate = Input.mousePosition;
                if(swipeInProgress == false){
                    moveUp();
                    swipeInProgress = true;
                }
            }
        } else if(mouseDeltaX == mouseDeltaY){
            //Debug.Log("inside false");
            swipeInProgress = false;
        }
    }

    void handleKeyboardInput(){
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            moveLeft();
        }

        if(Input.GetKeyDown(KeyCode.RightArrow)){
            moveRight();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow)){
            moveUp();
        }

        if(Input.GetKeyDown(KeyCode.DownArrow)){
           moveDown();
        }

        
    }

    private void CursorLock() //reset the Cursor by first locking it with this function and unlock it with the next on
    {
        if (swipeInProgress == false) //prevet interrupting a swipe
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void CursorUnlock()
    {
        if (Cursor.lockState == CursorLockMode.Locked) // If Cursor is Locked, unlock it to reset in the middle of the screen
        {
            Cursor.lockState = CursorLockMode.None;
            lastMouseCoordinate = Input.mousePosition; //prevent the false recognition of cursor reset as a swipe
        }
    }

    private void HideCursor()
    {
        Cursor.visible = false;
    }
}

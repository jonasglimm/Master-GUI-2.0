using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using TrackpadTouch;

/// <summary>
/// Not only Trackpad Control... Also control for iDrive Controller!!
/// </summary>

public class AuswahlTrackpad : MonoBehaviour{
    //assign each button to have the possibility to start a comparison after a button was clicked
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    private Button currentButton;
    private Vector3 lastMouseCoordinate = Vector3.zero; // used to store the last mose moved co-ordinates. Initialized with (0,0,0)
    private bool swipeInProgress = false;
    private Button[] buttonListForRotation = new Button[6]; //collect all buttons in on list to select for rotation with the iDrive-Controller
    
    private bool isTrackpadEnabled;
    private bool touchscreenInput;
    private bool iDriveInput;
    private bool gestureInput;

    private float cursorResetTime;
    public float swipeMovementX = 20f; //set a factor for the distance in x-directions after which a swipe is detected
    public float swipeMovementy = 20f; //set a factor for the distance in y-directions after which a swipe is detected
    public float swipeDistanceMagicTrackpad = 50f; //set a factor for the distance on the Apple Magic Trackpad after which a swipe is detected

    public ValueControlCenter valueControlCenter; 
    public AudioSource clickSound; // for a click (only used for TouchpadInput - for Touch and iDrive it is played via onClick() of the button
    public AudioSource scrollingSound;

    //For TrackpadSwipe()
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDragging, moved = false;
    private Vector2 startTouch, swipeDelta;

    //For IDriveController
    private IDriveController iDriveController;
    public bool rotationIsActive = true;

    void Awake()
    {
        //assign the local variables to global variable in ValueControlCenter
        isTrackpadEnabled = valueControlCenter.touchpadInput;
        touchscreenInput = valueControlCenter.touchscreenInput;
        iDriveInput = valueControlCenter.iDriveInput;
        gestureInput = valueControlCenter.gestureInput;

        cursorResetTime = valueControlCenter.cursorResetTime;
        iDriveController = GameObject.Find("AufgabenManager").GetComponent<IDriveController>();
    }

    void Start()
    {
        if (touchscreenInput == false)
        {
            SelectButton(valueControlCenter.startButton); //only if the input is not via Touchscreen, a startbutton is needed

            if (isTrackpadEnabled == true)
            {
                //InvokeRepeating("CursorLock", cursorResetTime, cursorResetTime);  // If the trackpad is used, the cursor will be reset to the middle of the screen each cursorResetTime - seconds
                HideCursor();
            }
        }
        if (!iDriveInput) //fill the button list for rotation via iDrive-Controller
        {
            iDriveController.enabled = false;
        }
        else
        {
            buttonListForRotation[0] = button1;
            buttonListForRotation[1] = button2;
            buttonListForRotation[2] = button3;
            buttonListForRotation[3] = button6;
            buttonListForRotation[4] = button5;
            buttonListForRotation[5] = button4;
        }
    }

    void SelectButton(Button btn) //on function to select a button
    {
        btn.Select(); 
        currentButton = btn;
    }
    void moveLeft(){ //move the selected button one element to the left, if is not at a left end
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
        /*
        if(currentButton != button1 && currentButton != button4)
        {
            scrollingSound.Play();
        }*/
    }

    void moveRight()
    { //move the selected button one element to the right, if is not at a right end
        if (currentButton == button1){
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
        /*
        if (currentButton != button3 && currentButton != button6)
        {
            scrollingSound.Play();
        }*/
    }

    void moveUp()
    {//move the selected button one element up, if is not at the upper end
        if (currentButton == button4){
            StartCoroutine(waiter(button1));
            // SelectButton(button1);
        } else if(currentButton == button5){
            StartCoroutine(waiter(button2));
            // SelectButton(button2);
        } else if(currentButton == button6){
            StartCoroutine(waiter(button3));
            // SelectButton(button3);
        }
        /*
        if (currentButton != button1 && currentButton != button2 && currentButton != button3)
        {
            scrollingSound.Play();
        }*/
    }

    void moveDown()
    {//move the selected button one element down, if is not at the lower end
        if (currentButton == button1){
            StartCoroutine(waiter(button4));
            // SelectButton(button4);
        } else if(currentButton == button2){
            StartCoroutine(waiter(button5));
            // SelectButton(button5);
        } else if(currentButton == button3){
            StartCoroutine(waiter(button6));
            // SelectButton(button6);
        }
        /*
        if (currentButton != button4 && currentButton != button5 && currentButton != button6)
        {
            scrollingSound.Play();
        }*/
    }
    
    
    IEnumerator waiter(Button btn){ //play a sound feedback and select the button
        yield return new WaitForSeconds(0.115f);
        if (currentButton != btn)
        {
            scrollingSound.Play();
        }
        SelectButton(btn);
    }


    // Update is called once per frame
    void Update()
    {
        if (isTrackpadEnabled == true){
            CursorUnlock(); // Unlock and reset Cursor if it is locked

            //handling trackpad swipe as input

            TrackpadSwipes();
            //handleTrackpadGesture(); //exchanged with TrackpadSwipes()
        }
        else if(iDriveInput == true){
            //handling keyboard arrrow keys as input
            handleKeyboardInput();
            handleIDriveControllerInput();
        }

    }

    public void TrackpadSwipes() //check for possible swiipes, the direction and the magnitude 
    {
        tap = moved = swipeDown = swipeLeft = swipeRight = swipeUp = false; //set each state to false

        if (TrackpadInput.touchCount > 0) //check the status of the touchcount
        {
            if (TrackpadInput.touches[0].phase == TouchPhase.Began)
            {
                isDragging = true;
                startTouch = TrackpadInput.touches[0].position; //save the touchposition in pixels
            }
            else if (TrackpadInput.touches[0].phase == TouchPhase.Ended || TrackpadInput.touches[0].phase == TouchPhase.Canceled)
            {
                isDragging = false;
                Reset();
            }
        }

        //calculate the distance of the swipe
        swipeDelta = Vector2.zero;
        if (isDragging)
        {
            if (TrackpadInput.touchCount > 0)
            {
                swipeDelta = TrackpadInput.touches[0].position - startTouch;
            }
        }

        //Did the finger move?
        if(swipeDelta.magnitude > 25)
        {
            moved = true;
        }

        //Did we cross the deadzone?
        if (swipeDelta.magnitude > swipeDistanceMagicTrackpad)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Left or right
                if (x < 0)
                {
                    swipeLeft = true;
                    moveLeft();
                    CursorLock();
                }
                else
                {
                    swipeRight = true;
                    moveRight();
                    CursorLock();
                }
            }
            else
            {
                //Up or down
                if (y < 0)
                {
                    swipeDown = true;
                    moveDown();
                    CursorLock();
                }
                else
                {
                    swipeUp = true;
                    moveUp();
                    CursorLock();
                }
            }
            Reset();
        }

        if (Input.GetMouseButtonDown(0)) //if the touchpad is pressed or taped
        {
            AuswahlControl script = gameObject.GetComponent<AuswahlControl>();
            script.Comparision(currentButton); //check if the correct button was clicked
            clickSound.Play();
            SelectButton(currentButton);
        }
        /*
        if (swipeLeft)
            Debug.Log("SwipeLeft");
        if (swipeRight)
            Debug.Log("SwipeRight");
        if (swipeUp)
            Debug.Log("SwipeUp");
        if (swipeDown)
            Debug.Log("SwipeDown");
        */
    }

    private void Reset() //reset all variables
    {
        if (moved == false)
        {
            if(swipeDown == swipeUp == swipeRight == swipeLeft == false)
            {
                tap = true;
            }
        }
        isDragging = false;
        startTouch = swipeDelta = Vector2.zero;
    }


    //Alternative to TrackpadSwipes()
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
            if(mouseDelta.x < - swipeMovementX){ // if difference less than zero, moved to left
                lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
                if(swipeInProgress == false){ // checking if the swipe gesture that had been started is still in progress or not. 
                    moveLeft();
                    CursorLock();
                    swipeInProgress = true; // swipe gesture is taking place
                }
                //prev value 15
            } else if(mouseDelta.x > swipeMovementX)
            { // if difference greater than zero, moved to right
                lastMouseCoordinate = Input.mousePosition;
                if(swipeInProgress == false){
                    moveRight();
                    CursorLock();
                    swipeInProgress = true;
                }
            }
        } else if((mouseDeltaX) < mouseDeltaY) {
             if(mouseDelta.y < -swipeMovementy){ // if difference less than zero, moved down
                lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
                if(swipeInProgress == false){ // checking if the swipe gesture that had been started is still in progress or not. 
                    moveDown();
                    CursorLock();
                    swipeInProgress = true; // swipe gesture is taking place
                }
            } else if(mouseDelta.y > swipeMovementy)
            { // if difference greater than zero, moved up
                lastMouseCoordinate = Input.mousePosition;
                if(swipeInProgress == false){
                    moveUp();
                    CursorLock();
                    swipeInProgress = true;
                }
            }
        } else if(mouseDeltaX == mouseDeltaY){
            //Debug.Log("inside false");
            swipeInProgress = false;
        }
    }

    //Use keyboard input as a example to check the functionality
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

    private void handleIDriveControllerInput() //handle movement of Controller to left, right, up and down
    {
        if (iDriveController.movedLeftOnce)
        {
            moveLeft();
        }
        if (iDriveController.movedRightOnce)
        {
            moveRight();
        }
        if (iDriveController.movedUpOnce)
        {
            moveUp();
        }
        if (iDriveController.movedDownOnce)
        {
            moveDown();
        }
        if (iDriveController.pushedOnce)
        {
            AuswahlControl script = gameObject.GetComponent<AuswahlControl>();
            script.Comparision(currentButton);
            clickSound.Play();
            SelectButton(currentButton);
        }
        //
        // Integrate rotation - there is surely a smarter way - was a late change
        //

        #region Buttonrotation
        
        if (rotationIsActive)
        {
            if(iDriveController.rotationClockwiseSteps > 0 || iDriveController.rotationCounterclockwiseSteps > 0)
            {
                int counter = 0;

                if(currentButton == button1) //counter is needed to scroll through the button in a circular order
                {
                    counter = 0;
                }
                else if (currentButton == button2)
                {
                    counter = 1;
                }
                else if (currentButton == button3)
                {
                    counter = 2;
                }
                else if (currentButton == button6)
                {
                    counter = 3;
                }
                else if(currentButton == button5)
                {
                    counter = 4;
                }
                else if (currentButton == button4)
                {
                    counter = 5;
                }

                if(iDriveController.rotationClockwiseSteps > 0)
                {
                    if(counter + iDriveController.rotationClockwiseSteps < 6) //check if the rotation was very quick and the iDrive-Controller was rotated for more than 6 steps in on frame
                    {
                        counter = counter + iDriveController.rotationClockwiseSteps;
                    }
                    else if(counter + iDriveController.rotationClockwiseSteps < 12)
                    {
                        counter = counter + iDriveController.rotationClockwiseSteps - 6;
                    }
                }
                if (iDriveController.rotationCounterclockwiseSteps > 0)
                {
                    if (counter - iDriveController.rotationCounterclockwiseSteps >= 0)
                    {
                        counter = counter - iDriveController.rotationCounterclockwiseSteps;
                    }
                    else if (counter - iDriveController.rotationCounterclockwiseSteps > -6)
                    {
                        counter = counter - iDriveController.rotationCounterclockwiseSteps + 6;
                    }
                }
                //currentButton = buttonListForRotation[counter];
                SelectButton(buttonListForRotation[counter]); 
                scrollingSound.Play(); //acustic feedback
            }
        }
        

        #region Alternative
        /*
        if (rotationIsActive)
        {
            if (currentButton == button1)
            {
                if (iDriveController.turnedClockwise)
                {
                    moveRight();
                }
                else if (iDriveController.turnedCounterclockwise)
                {
                    moveDown();
                }
            }

            if (currentButton == button2)
            {
                if (iDriveController.turnedClockwise)
                {
                    moveRight();
                }
                else if (iDriveController.turnedCounterclockwise)
                {
                    moveLeft();
                }
            }

            if (currentButton == button3)
            {
                if (iDriveController.turnedClockwise)
                {
                    moveDown();
                }
                else if (iDriveController.turnedCounterclockwise)
                {
                    moveLeft();
                }
            }

            if (currentButton == button4)
            {
                if (iDriveController.turnedClockwise)
                {
                    moveUp();
                }
                else if (iDriveController.turnedCounterclockwise)
                {
                    moveRight();
                }
            }

            if (currentButton == button5)
            {
                if (iDriveController.turnedClockwise)
                {
                    moveLeft();
                }
                else if (iDriveController.turnedCounterclockwise)
                {
                    moveRight();
                }
            }

            if (currentButton == button6)
            {
                if (iDriveController.turnedClockwise)
                {
                    moveLeft();
                }
                else if (iDriveController.turnedCounterclockwise)
                {
                    moveUp();
                }
            }
        */
        #endregion

        #region Alternative2
        /*
        if (currentButton == button1)
        {
            if(iDriveController.rotationClockwiseSteps > 0)
            {
                if(iDriveController.rotationClockwiseSteps == 1)
                {
                    moveRight();
                }
                else if (iDriveController.rotationClockwiseSteps == 2)
                {
                    moveRight();
                    moveRight();
                }
                else if (iDriveController.rotationClockwiseSteps >= 3)
                {
                    moveRight();
                    moveRight();
                    moveDown();
                }
            }
        }
        else if(iDriveController.rotationCounterclockwiseSteps > 0)
        {
            if (iDriveController.rotationCounterclockwiseSteps == 1)
            {
                moveDown();
            }
            else if (iDriveController.rotationCounterclockwiseSteps == 2)
            {
                moveDown();
                moveRight();
            }
            else if (iDriveController.rotationCounterclockwiseSteps >= 3)
            {
                moveDown();
                moveRight();
                moveRight();
            }

        }

        if (currentButton == button2)
        {
            if (iDriveController.rotationClockwiseSteps > 0)
            {
                if (iDriveController.rotationClockwiseSteps == 1)
                {
                    moveRight();
                }
                else if (iDriveController.rotationClockwiseSteps == 2)
                {
                    moveRight();
                    moveDown();
                }
                else if (iDriveController.rotationClockwiseSteps >= 3)
                {
                    moveRight();
                    moveDown();
                    moveLeft();
                }
            }
        }
        else if (iDriveController.rotationCounterclockwiseSteps > 0)
        {
            if (iDriveController.rotationCounterclockwiseSteps == 1)
            {
                moveLeft();
            }
            else if (iDriveController.rotationCounterclockwiseSteps == 2)
            {
                moveLeft();
                moveDown();
            }
            else if (iDriveController.rotationCounterclockwiseSteps >= 3)
            {
                moveLeft();
                moveDown();
                moveRight();
            }

        }

        if (currentButton == button3)
        {
            if (iDriveController.rotationClockwiseSteps > 0)
            {
                if (iDriveController.rotationClockwiseSteps == 1)
                {
                    moveDown();
                }
                else if (iDriveController.rotationClockwiseSteps == 2)
                {
                    moveDown();
                    moveLeft();
                }
                else if (iDriveController.rotationClockwiseSteps >= 3)
                {
                    moveDown();
                    moveLeft();
                    moveLeft();
                }
            }
        }
        else if (iDriveController.rotationCounterclockwiseSteps > 0)
        {
            if (iDriveController.rotationCounterclockwiseSteps == 1)
            {
                moveLeft();
            }
            else if (iDriveController.rotationCounterclockwiseSteps == 2)
            {
                moveLeft();
                moveLeft();
            }
            else if (iDriveController.rotationCounterclockwiseSteps >= 3)
            {
                moveLeft();
                moveLeft();
                moveDown();
            }
        }
        //
        if (currentButton == button4)
        {
            if (iDriveController.rotationClockwiseSteps > 0)
            {
                if (iDriveController.rotationClockwiseSteps == 1)
                {
                    moveUp();
                }
                else if (iDriveController.rotationClockwiseSteps == 2)
                {
                    moveUp();
                    moveRight();
                }
                else if (iDriveController.rotationClockwiseSteps >= 3)
                {
                    moveUp();
                    moveRight();
                    moveRight();
                }
            }
        }
        else if (iDriveController.rotationCounterclockwiseSteps > 0)
        {
            if (iDriveController.rotationCounterclockwiseSteps == 1)
            {
                moveRight();
            }
            else if (iDriveController.rotationCounterclockwiseSteps == 2)
            {
                moveRight();
                moveRight();
            }
            else if (iDriveController.rotationCounterclockwiseSteps >= 3)
            {
                moveRight();
                moveRight();
                moveUp();
            }

        }

        if (currentButton == button5)
        {
            if (iDriveController.rotationClockwiseSteps > 0)
            {
                if (iDriveController.rotationClockwiseSteps == 1)
                {
                    moveLeft();
                }
                else if (iDriveController.rotationClockwiseSteps == 2)
                {
                    moveLeft();
                    moveUp();
                }
                else if (iDriveController.rotationClockwiseSteps >= 3)
                {
                    moveLeft();
                    moveUp();
                    moveRight();
                }
            }
        }
        else if (iDriveController.rotationCounterclockwiseSteps > 0)
        {
            if (iDriveController.rotationCounterclockwiseSteps == 1)
            {
                moveRight();
            }
            else if (iDriveController.rotationCounterclockwiseSteps == 2)
            {
                moveRight();
                moveUp();
            }
            else if (iDriveController.rotationCounterclockwiseSteps >= 3)
            {
                moveRight();
                moveUp();
                moveLeft();
            }

        }

        if (currentButton == button6)
        {
            if (iDriveController.rotationClockwiseSteps > 0)
            {
                if (iDriveController.rotationClockwiseSteps == 1)
                {
                    moveLeft();
                }
                else if (iDriveController.rotationClockwiseSteps == 2)
                {
                    moveLeft();
                    moveLeft();
                }
                else if (iDriveController.rotationClockwiseSteps >= 3)
                {
                    moveLeft();
                    moveLeft();
                    moveUp();
                }
            }
        }
        else if (iDriveController.rotationCounterclockwiseSteps > 0)
        {
            if (iDriveController.rotationCounterclockwiseSteps == 1)
            {
                moveUp();
            }
            else if (iDriveController.rotationCounterclockwiseSteps == 2)
            {
                moveUp();
                moveLeft();
            }
            else if (iDriveController.rotationCounterclockwiseSteps >= 3)
            {
                moveUp();
                moveLeft();
                moveLeft();
            }

        }
        */

        #endregion
#endregion
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

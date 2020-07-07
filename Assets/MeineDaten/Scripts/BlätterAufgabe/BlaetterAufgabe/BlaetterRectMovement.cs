using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using TrackpadTouch;

// this class moves the current button into view when the task is done using buttons
// needs tio be assigned to the SnapOnScroll-Gameobject (the gameobject with an scroll rect)
public class BlaetterRectMovement : MonoBehaviour
// not using Start() because array of Buttons needs to be intiated before this SetUp() calls.
//Gets called from ButtonListBlaettern - Script
{
    public ScrollRect scrollRect;
    public int startButton = 0;
    public float lerpTime = 0.1f;

    private Button[] buttons;
    private int numberOfButtons;
    private int index;
    private Vector2 position = new Vector2(0.5f, 0.5f);
    private Vector2 positionSetTo = new Vector2(0.5f, 0.5f);
    public Button selectedButton;

    //For SwipeTest()
    private bool tap, swipeLeft, swipeRight;
    private bool isDragging, moved = false;
    private Vector2 startTouch, swipeDelta;

    //For iDriveController
    private IDriveController iDriveController;


    //-------------- PageSelection
    public Sprite unselectedPage;
    public Sprite selectedPage;
    public Transform pageSelectionIcons;

    private bool isTrackpadEnabled; //set in ValueControlCenter
    private bool iDriveInput; //set in ValueControlCenter
    private bool gestureInput; //set in ValueControlCenter

    private bool _showPageSelection;
    private int _previousPageSelectionIndex;
    // container with Image components - one Image for each page
    private List<Image> _pageSelectionImages;
    private Vector3 lastMouseCoordinate = Vector3.zero;
    private bool swipeInProgress = false;
    public TextMeshProUGUI[] buttonText;
    public float swipeMovementX;
    public float swipeDistanceMagicTrackpad = 50f;

    private float cursorResetTime; //set in ValueControlCenter
    private int counter =0;
    private ValueControlCenter valueControlCenter;

    public AudioSource scrollingSound;

    public void Awake()
    {
        valueControlCenter = GameObject.Find("BlaetterManager").GetComponent<ValueControlCenter>();
        iDriveController = GameObject.Find("BlaetterManager").GetComponent<IDriveController>();

        cursorResetTime = valueControlCenter.cursorResetTime;
        isTrackpadEnabled = valueControlCenter.touchpadInput;
        iDriveInput = valueControlCenter.iDriveInput;
        gestureInput = valueControlCenter.gestureInput;
    }

    public void Start()
    {
        if (isTrackpadEnabled == true) // If the trackpad is used, the cursor will be reset to the middle of the screen each cursorResetTime - seconds
        {
            //InvokeRepeating("CursorLock", cursorResetTime, cursorResetTime);
            HideCursor();
        }
        if (!valueControlCenter.iDriveInput)
        {
            iDriveController.enabled = false;
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

    public void SetUp()
    {
        buttons = GetComponentsInChildren<Button>();
        numberOfButtons = buttons.Length;
        index = startButton;
        selectedButton = buttons[index];
        buttons[index].Select();

        position.x = ((float)index / (buttons.Length - 1));
        InitPageSelection();
        SetPageSelection(startButton);

    }

    public void TrackpadSwipes()
    {
        tap = moved = swipeLeft = swipeRight = false;

        if (TrackpadInput.touchCount > 0)
        {
            if (TrackpadInput.touches[0].phase == TouchPhase.Began)
            {
                isDragging = true;
                startTouch = TrackpadInput.touches[0].position;
            }
            else if (TrackpadInput.touches[0].phase == TouchPhase.Ended || TrackpadInput.touches[0].phase == TouchPhase.Canceled)
            {
                isDragging = false;
                Reset();
            }
        }

        //calculate the distance
        swipeDelta = Vector2.zero;
        if (isDragging)
        {
            if (TrackpadInput.touchCount > 0)
            {
                swipeDelta = TrackpadInput.touches[0].position - startTouch;
            }
        }

        //Did the finger move?
        if (swipeDelta.magnitude > 50)
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
                if (x > 0)
                {
                    swipeRight = true;
                    if (index > 0)
                    {
                        index--;
                        selectedButton = buttons[index];
                        buttons[index].Select();
                        scrollingSound.Play();
                    }
                    else if (index == 0)
                    {
                        index = numberOfButtons - 1;
                        selectedButton = buttons[index];
                        buttons[index].Select();
                        scrollingSound.Play();
                    }
                    CursorLock();
                }
                else
                {
                    swipeLeft = true;
                    if (index < numberOfButtons - 1)
                    {
                        index++;
                        selectedButton = buttons[index];
                        buttons[index].Select();
                        scrollingSound.Play();
                    }
                    else if (index == numberOfButtons - 1)
                    {
                        index = 0;
                        selectedButton = buttons[index];
                        buttons[index].Select();
                        scrollingSound.Play();
                    }
                    CursorLock();
                }
            }
            Reset();
        }

        /*
        if (swipeLeft)
            Debug.Log("SwipeLeft");
        if (swipeRight)
            Debug.Log("SwipeRight");
        */
    }

    private void Reset()
    {
        if (moved == false)
        {
            if (swipeRight == swipeLeft == false)
            {
                tap = true;
            }
        }
        isDragging = false;
        startTouch = swipeDelta = Vector2.zero;
    }

    private void handleSwipe(){
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        if(mouseDelta.x > swipeMovementX){ // if difference less than zero, moved to left
            if(swipeInProgress == false ){ // checking if the swipe gesture that had been started is still in progress or not.         
                swipeInProgress = true;   
                if(index > 0){
                    index--; 
                    selectedButton = buttons[index];   
                    buttons[index].Select();
                    scrollingSound.Play();
                } 
                else if (index == 0)
                {
                    index = numberOfButtons - 1;
                    selectedButton = buttons[index];
                    buttons[index].Select();
                    scrollingSound.Play();
                } 
            }
        } else if(mouseDelta.x < -swipeMovementX){ // if difference greater than zero, moved to right      
            if(swipeInProgress == false ){
                swipeInProgress = true;   
                 if(index < numberOfButtons-1){
                    index++; 
                    selectedButton = buttons[index];    
                    buttons[index].Select();           
                    scrollingSound.Play();
                }
                else if (index == numberOfButtons -1)
                {
                    index = 0;
                    selectedButton = buttons[index];
                    buttons[index].Select();
                    scrollingSound.Play();
                }
            }
        } else if(mouseDelta.x == 0) {
            swipeInProgress = false;
        }
        if(swipeInProgress == true){
            if(counter == 20){
                lastMouseCoordinate = Input.mousePosition;
                counter = 0;
            }
            counter++;
        }
    }


        void Update(){
        if(isTrackpadEnabled == true){
            CursorUnlock();
            TrackpadSwipes();
            //handleSwipe(); //exchanges with TrackpadSwipes()
        } else {
            selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            index = System.Array.IndexOf(buttons, selectedButton);
            IDriveScrolling();
            buttons[index].Select();
        }
        buttonText = selectedButton.GetComponentsInChildren<TextMeshProUGUI>();
        position.x = ((float)index / (buttons.Length - 1));  
        positionSetTo.x = Mathf.Lerp(scrollRect.normalizedPosition.x, position.x, Time.deltaTime / lerpTime);
        scrollRect.normalizedPosition = positionSetTo;
        SetPageSelection(index);
       
    }

   private void IDriveScrolling()
    {
        if (iDriveInput == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) | Input.GetKeyDown(KeyCode.RightArrow)) //keys must be changed for actual iDrive-Controller
            {
                scrollingSound.Play();
            }

            if (iDriveController.movedRightOnce)
            {
                if (index < numberOfButtons - 1)
                {
                    index++;
                    selectedButton = buttons[index];
                    buttons[index].Select();
                    scrollingSound.Play();
                }
                else if (index == numberOfButtons - 1)
                {
                    index = 0;
                    selectedButton = buttons[index];
                    buttons[index].Select();
                    scrollingSound.Play();
                }
            }
            else if (iDriveController.movedLeftOnce)
            {
                if (index > 0)
                {
                    index--;
                    selectedButton = buttons[index];
                    buttons[index].Select();
                    scrollingSound.Play();
                }
                else if (index == 0)
                {
                    index = numberOfButtons - 1;
                    selectedButton = buttons[index];
                    buttons[index].Select();
                    scrollingSound.Play();
                }
            }
        }
    }

    private void InitPageSelection()
    {
        // page selection - only if defined sprites for selection icons
        _showPageSelection = unselectedPage != null && selectedPage != null;
        if (_showPageSelection)
        {
            // also container with selection images must be defined and must have exatly the same amount of items as pages container
            if (pageSelectionIcons == null || pageSelectionIcons.childCount != buttons.Length)
            {
                Debug.LogWarning("Different count of pages and selection icons - will not show page selection");
                _showPageSelection = false;
            }
            else
            {
                _previousPageSelectionIndex = -1;
                _pageSelectionImages = new List<Image>();

                // cache all Image components into list
                for (int i = 0; i < pageSelectionIcons.childCount; i++)
                {
                    Image image = pageSelectionIcons.GetChild(i).GetComponent<Image>();
                    if (image == null)
                    {
                        Debug.LogWarning("Page selection icon at position " + i + " is missing Image component");
                    }
                    _pageSelectionImages.Add(image);
                }
            }
        }
    }

    //------------------------------------------------------------------------
    private void SetPageSelection(int aPageIndex)
    {
        // nothing to change
        if (_previousPageSelectionIndex == aPageIndex)
        {
            return;
        }

        // unselect old
        if (_previousPageSelectionIndex >= 0)
        {
            _pageSelectionImages[_previousPageSelectionIndex].sprite = unselectedPage;
            _pageSelectionImages[_previousPageSelectionIndex].SetNativeSize();
        }

        // select new
        _pageSelectionImages[aPageIndex].sprite = selectedPage;
        _pageSelectionImages[aPageIndex].SetNativeSize();

        _previousPageSelectionIndex = aPageIndex;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TrackpadTextInsertion : MonoBehaviour{
    [Header("GUI Elements")]
    public TMP_InputField inputField;
    public Image selector;
    public int minSelectorX;
    public int maxSelectorX;
    public int minSelectorY;
    public int maxSelectorY;

    [Header("Swipe Movement")]
    public float swipeDistanceX;
    public float swipeDistanceY;

    [Header("Sounds")]
    public AudioSource clickSound;
    public AudioSource scrollSound;

    private Vector3 lastMouseCoordinate = Vector3.zero;
    private RectTransform selectorRectTransform;
    // Start is called before the first frame update
    private bool swipeInProgress = false;
    private float cursorResetTime;
    private ControlManager script;
    void Start(){
        selectorRectTransform = selector.GetComponent<RectTransform>();
        script = gameObject.GetComponent<ControlManager>();
        cursorResetTime = script.cursorResetTime;

        if(script.touchscreenInput == true ||  script.touchpadInput == true ){
            InvokeRepeating("CursorLock", cursorResetTime, cursorResetTime);  // If the trackpad is used, the cursor will be reset to the middle of the screen each cursorResetTime - seconds
            HideCursor();
        }
    }
     void insertText(string text){
        inputField.text = inputField.text + text;
    }

    // Update is called once per frame
    void Update(){
        if(script.touchscreenInput == true ||  script.touchpadInput == true ){
             CursorUnlock();
            Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
            handleSwipeGesture(mouseDelta);
            // handleHorizontalGesture(mouseDelta);
            if(Input.GetMouseButtonDown(0)){
                if(selectorRectTransform.localPosition.x == minSelectorX && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("A");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + 70) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("B");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (2*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("C");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (3*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("D");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (4*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("E");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (5*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("F");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (6*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("G");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (7*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("H");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (8*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("I");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (9*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("J");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (10*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("K");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (11*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("L");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (12*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("M");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (13*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("N");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (14*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    insertText("O");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (15*70)) && selectorRectTransform.localPosition.y == maxSelectorY){
                    string temp = inputField.text;
                    if(temp.Length != 0){
                        clickSound.Play();
                        char lastChar = temp[temp.Length-1];
                        if(lastChar == ' '){
                            inputField.text =  temp.Substring(0,temp.Length-2);
                        } else {
                            inputField.text =  temp.Substring(0,temp.Length-1);
                        }
                    }
                } else if(selectorRectTransform.localPosition.x == (minSelectorX) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("P");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (1*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("Q");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (2*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("R");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (3*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("S");
                    clickSound.Play();
                }  else if(selectorRectTransform.localPosition.x == (minSelectorX + (4*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("T");
                    clickSound.Play();
                }  else if(selectorRectTransform.localPosition.x == (minSelectorX + (5*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("U");
                    clickSound.Play();
                }  else if(selectorRectTransform.localPosition.x == (minSelectorX + (6*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("V");
                    clickSound.Play();
                }  else if(selectorRectTransform.localPosition.x == (minSelectorX + (7*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("W");
                    clickSound.Play();
                }  else if(selectorRectTransform.localPosition.x == (minSelectorX + (8*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("X");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (9*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("Y");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (10*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("Z");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (11*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("Ä");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (12*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("Ö");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (13*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText("Ü");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (14*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    insertText(" ");
                    clickSound.Play();
                } else if(selectorRectTransform.localPosition.x == (minSelectorX + (15*70)) && selectorRectTransform.localPosition.y == minSelectorY){
                    // Enter value
                    if(Input.GetMouseButtonDown(0)){
                        ControlManager script = gameObject.GetComponent<ControlManager>();
                        script.checkOutput(inputField.text);
                        inputField.text = "";
                    }
                }
            }
        }
    }

    void handleSwipeGesture(Vector3 mouseDelta){
        // Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        //value 20 works for normal move ment without gesture for both directions
        float mouseDeltaX = mouseDelta.x;
        float mouseDeltaY = mouseDelta.y;
        
        if(mouseDeltaY < 0){
            mouseDeltaY = -mouseDeltaY;
        }
        if(mouseDeltaX < 0){
            mouseDeltaX = -mouseDeltaX;
        }
        if((mouseDeltaX) > mouseDeltaY){
            if(mouseDelta.x < -swipeDistanceX){ // if difference less than zero, moved to left
            // horizontalMovementTxt.text = "Mouse Moved Left";
                lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
                if(selectorRectTransform.localPosition.x > minSelectorX){
                    scrollSound.Play();
                    selectorRectTransform.localPosition = new Vector3(selectorRectTransform.localPosition.x-70, selectorRectTransform.localPosition.y, selectorRectTransform.localPosition.z);
                }
            } else if(mouseDelta.x > swipeDistanceX){ // if difference greater than zero, moved to right
                lastMouseCoordinate = Input.mousePosition;
                if(selectorRectTransform.localPosition.x < maxSelectorX){
                    scrollSound.Play();
                    selectorRectTransform.localPosition = new Vector3(selectorRectTransform.localPosition.x+70, selectorRectTransform.localPosition.y, selectorRectTransform.localPosition.z);
                }
            } 
        } else if((mouseDeltaX) < mouseDeltaY) {
             if(mouseDelta.y < -swipeDistanceY){ // if difference less than zero, moved down
                lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
                if(swipeInProgress == false){ // checking if the swipe gesture that had been started is still in progress or not. 
                    // moveDown();
                    if(selectorRectTransform.localPosition.y != minSelectorY){
                        selectorRectTransform.localPosition = new Vector3(selectorRectTransform.localPosition.x, minSelectorY, selectorRectTransform.localPosition.z);
                        scrollSound.Play();
                    }
                    swipeInProgress = true; // swipe gesture is taking place
                }
            } else if(mouseDelta.y > swipeDistanceY){ // if difference greater than zero, moved up
                lastMouseCoordinate = Input.mousePosition;
                if(swipeInProgress == false){
                    // moveUp();
                    if(selectorRectTransform.localPosition.y != maxSelectorY){
                        selectorRectTransform.localPosition = new Vector3(selectorRectTransform.localPosition.x, maxSelectorY, selectorRectTransform.localPosition.z);
                        scrollSound.Play();
                    }
                    swipeInProgress = true;
                }
            }
        } else if(mouseDeltaX == mouseDeltaY){
            swipeInProgress = false;
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

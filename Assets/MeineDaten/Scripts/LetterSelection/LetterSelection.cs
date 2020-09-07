using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LetterSelection : MonoBehaviour
{
    //drag according GUI-elements into the Inspector
    public Image selector;
    public TMP_InputField inputField;
    public TextMeshProUGUI selectorText;
    public GameObject returnImage;
    public GameObject backspaceImage;
    public GameObject knob;
    public AudioSource scrollingSound;
    public AudioSource clickSound;
    //assign local variable to public classes
    private ControlManager script;
    private IDriveController iDriveController;

    private void Start() {
        selector.transform.eulerAngles =  new Vector3(selector.transform.eulerAngles.x, selector.transform.eulerAngles.y, 0f); //initiate a vector to discribe the rotation of the selector
        inputField.text  = ""; //empty the input field
        //assign local variable to public classes
        script = gameObject.GetComponent<ControlManager>(); 
        iDriveController = GameObject.Find("Manager").GetComponent<IDriveController>();
        //activate or deactivate the presented knob
        if (script.touchscreenInput == true)
        {
            knob.SetActive(false);
        }
        if (!script.iDriveInput)
        {
            iDriveController.enabled = false;
        }
    }

    private void Update() {
        if(script.iDriveInput == true){

            if(iDriveController.turnedClockwise)
            {
                scrollingSound.Play();
                selector.transform.eulerAngles = new Vector3(selector.transform.eulerAngles.x, selector.transform.eulerAngles.y, selector.transform.eulerAngles.z - (iDriveController.rotationClockwiseSteps * 11.25f)); // rotate the selector for 11.25 degree each rotatoin step
            }
            else if(iDriveController.turnedCounterclockwise)
            {
                scrollingSound.Play();
                selector.transform.eulerAngles = new Vector3(selector.transform.eulerAngles.x, selector.transform.eulerAngles.y, selector.transform.eulerAngles.z + (iDriveController.rotationCounterclockwiseSteps * 11.25f));
            }
            /*
            if (Input.mouseScrollDelta.y > 0){
                scrollingSound.Play();
                selector.transform.eulerAngles =  new Vector3(selector.transform.eulerAngles.x, selector.transform.eulerAngles.y, selector.transform.eulerAngles.z + 11.25f);
            }
            else if(Input.mouseScrollDelta.y < 0){
                scrollingSound.Play();
                selector.transform.eulerAngles =  new Vector3(selector.transform.eulerAngles.x, selector.transform.eulerAngles.y, selector.transform.eulerAngles.z - 11.25f);
            }*/


            void insertText(string text) //function to add a letter or textelement to the inputfield
            {
                inputField.text = inputField.text + text;
            }


            //if the controller is lateraly moved, the cursor jumpes on digit within the input field
            if (iDriveController.movedLeftOnce) 
            {
                inputField.caretPosition = inputField.caretPosition - 1;
            }
            else if (iDriveController.movedRightOnce)
            {
                inputField.caretPosition = inputField.caretPosition + 1;
            }
            //
            //depending on the angle, the selector text is changed according to the selected letter
            //if the iDrive-Controller is pressed, the according letter is added to the input field
            //if the last input was the return or backspace image, it needs to be deactivated
            //
            if (selector.transform.eulerAngles.z >= 0f-1f && selector.transform.eulerAngles.z < 0f+1f ){
                returnImage.SetActive(false); 
                backspaceImage.SetActive(false);
                selectorText.text = "A";
                if(iDriveController.pushedOnce)
                {
                    insertText("A");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 348.75f-1f && selector.transform.eulerAngles.z < 348.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "B";
                if(iDriveController.pushedOnce)
                {
                    insertText("B");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 337.5f-1f && selector.transform.eulerAngles.z < 337.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "C";
                if(iDriveController.pushedOnce)
                {
                    insertText("C");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 326.25f-1f && selector.transform.eulerAngles.z < 326.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "D";
                if(iDriveController.pushedOnce)
                {
                    insertText("D");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 315f-1f && selector.transform.eulerAngles.z < 315f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "E";
                if(iDriveController.pushedOnce)
                {
                    insertText("E");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 303.75f-1f && selector.transform.eulerAngles.z < 303.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "F";
                if(iDriveController.pushedOnce)
                {
                    insertText("F");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 292.5f-1f && selector.transform.eulerAngles.z < 292.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "G";
                if(iDriveController.pushedOnce)
                {
                    insertText("G");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 281.25f-1f && selector.transform.eulerAngles.z < 281.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "H";
                if(iDriveController.pushedOnce)
                {
                    insertText("H");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 270f-1f && selector.transform.eulerAngles.z < 270f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "I";
                if(iDriveController.pushedOnce)
                {
                    insertText("I");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 258.75f-1f && selector.transform.eulerAngles.z < 258.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "J";
                if(iDriveController.pushedOnce)
                {
                    insertText("J");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 247.5f-1f && selector.transform.eulerAngles.z < 247.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "K";
                if(iDriveController.pushedOnce)
                {
                    insertText("K");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 236.25f-1f && selector.transform.eulerAngles.z < 236.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "L";
                if(iDriveController.pushedOnce)
                {
                    insertText("L");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 225f-1f && selector.transform.eulerAngles.z < 225f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "M";
                if(iDriveController.pushedOnce)
                {
                    insertText("M");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 213.75f-1f && selector.transform.eulerAngles.z < 213.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "N";
                if(iDriveController.pushedOnce)
                {
                    insertText("N");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 202.5f-1f && selector.transform.eulerAngles.z < 202.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "O";
                if(iDriveController.pushedOnce)
                {
                    insertText("O");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 191.25f-1f && selector.transform.eulerAngles.z < 191.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "P";
                if(iDriveController.pushedOnce)
                {
                    insertText("P");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 180f-1f && selector.transform.eulerAngles.z < 180f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Q";
                if(iDriveController.pushedOnce)
                {
                    insertText("Q");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 168.75f-1f && selector.transform.eulerAngles.z < 168.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "R";
                if(iDriveController.pushedOnce)
                {
                    insertText("R");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 157.5f-1f && selector.transform.eulerAngles.z < 157.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "S";
                if(iDriveController.pushedOnce)
                {
                    insertText("S");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 146.25f-1f && selector.transform.eulerAngles.z < 146f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "T";
                if(iDriveController.pushedOnce)
                {
                    insertText("T");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 135f-1f && selector.transform.eulerAngles.z < 135f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "U";
                if(iDriveController.pushedOnce)
                {
                    insertText("U");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 123.75f-1f && selector.transform.eulerAngles.z < 123.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "V";
                if(iDriveController.pushedOnce)
                {
                    insertText("V");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 112.5f-1f && selector.transform.eulerAngles.z < 112.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "W";
                if(iDriveController.pushedOnce)
                {
                    insertText("W");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 101.25f-1f && selector.transform.eulerAngles.z < 101.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "X";
                if(iDriveController.pushedOnce)
                {
                    insertText("X");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 90f-1f && selector.transform.eulerAngles.z < 90f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Y";
                if(iDriveController.pushedOnce)
                {
                    insertText("Y");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 78.75f-1f && selector.transform.eulerAngles.z < 78.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Z";
                if(iDriveController.pushedOnce)
                {
                    insertText("Z");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 67.5f-1f && selector.transform.eulerAngles.z < 67.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Ä";
                if(iDriveController.pushedOnce)
                {
                    insertText("Ä");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 56.25f-1f && selector.transform.eulerAngles.z < 56.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Ö";
                if(iDriveController.pushedOnce)
                {
                    insertText("Ö");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 45f-1f && selector.transform.eulerAngles.z < 45f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Ü";
                if(iDriveController.pushedOnce)
                {
                    insertText("Ü");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 11.25f-1f && selector.transform.eulerAngles.z < 11.25f+1f) 
            {
                //add a blank space
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "_";

                if(iDriveController.pushedOnce)
                {
                    insertText(" ");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 22.5f-1f && selector.transform.eulerAngles.z < 22.5f+1f){
                // Enter value
                selectorText.text = "";
                returnImage.SetActive(true);
                backspaceImage.SetActive(false);

                if(iDriveController.pushedOnce){
                    script.checkOutput(inputField.text);
                    inputField.text  = "";
                }
            } else if(selector.transform.eulerAngles.z >= 33.75f-1f && selector.transform.eulerAngles.z < 33.75f+1f){
                //backspace
                selectorText.text = "";
                returnImage.SetActive(false);
                backspaceImage.SetActive(true);

                if(iDriveController.pushedOnce){
                    string temp = inputField.text;
                    if(temp.Length != 0) //check if the input field has content 
                    {
                        clickSound.Play();
                        char lastChar = temp[temp.Length-1];
                        if(lastChar == ' '){
                            inputField.text =  temp.Substring(0,temp.Length-2);
                        } else {
                            inputField.text =  temp.Substring(0,temp.Length-1);
                        }
                    }
                }
            } 
        }
        
    }
}

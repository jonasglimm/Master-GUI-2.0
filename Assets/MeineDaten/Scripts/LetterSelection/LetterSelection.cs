using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LetterSelection : MonoBehaviour
{
    public Image selector;
    public TMP_InputField inputField;
    public TextMeshProUGUI selectorText;
    public GameObject returnImage;
    public GameObject backspaceImage;
    public AudioSource scrollingSound;
    public AudioSource clickSound;
    private ControlManager script;
    private void Start() {
        selector.transform.eulerAngles =  new Vector3(selector.transform.eulerAngles.x, selector.transform.eulerAngles.y, 0f);
        inputField.text  = "";
        script = gameObject.GetComponent<ControlManager>();
        
    }
    private void Update() {
        if(script.iDriveInput == true){
            if(Input.mouseScrollDelta.y > 0){
                scrollingSound.Play();
                selector.transform.eulerAngles =  new Vector3(selector.transform.eulerAngles.x, selector.transform.eulerAngles.y, selector.transform.eulerAngles.z + 11.25f);
            }

            if(Input.mouseScrollDelta.y < 0){
                scrollingSound.Play();
                selector.transform.eulerAngles =  new Vector3(selector.transform.eulerAngles.x, selector.transform.eulerAngles.y, selector.transform.eulerAngles.z - 11.25f);
            }
            void insertText(string text){
                inputField.text = inputField.text + text;
            }

        
            if(selector.transform.eulerAngles.z >= 0f-1f && selector.transform.eulerAngles.z < 0f+1f ){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "A";
                if(Input.GetMouseButtonDown(0)){
                    insertText("A");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 348.75f-1f && selector.transform.eulerAngles.z < 348.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "B";
                if(Input.GetMouseButtonDown(0)){
                    insertText("B");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 337.5f-1f && selector.transform.eulerAngles.z < 337.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "C";
                if(Input.GetMouseButtonDown(0)){
                    insertText("C");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 326.25f-1f && selector.transform.eulerAngles.z < 326.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "D";
                if(Input.GetMouseButtonDown(0)){
                    insertText("D");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 315f-1f && selector.transform.eulerAngles.z < 315f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "E";
                if(Input.GetMouseButtonDown(0)){
                    insertText("E");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 303.75f-1f && selector.transform.eulerAngles.z < 303.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "F";
                if(Input.GetMouseButtonDown(0)){
                    insertText("F");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 292.5f-1f && selector.transform.eulerAngles.z < 292.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "G";
                if(Input.GetMouseButtonDown(0)){
                    insertText("G");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 281.25f-1f && selector.transform.eulerAngles.z < 281.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "H";
                if(Input.GetMouseButtonDown(0)){
                    insertText("H");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 270f-1f && selector.transform.eulerAngles.z < 270f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "I";
                if(Input.GetMouseButtonDown(0)){
                    insertText("I");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 258.75f-1f && selector.transform.eulerAngles.z < 258.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "J";
                if(Input.GetMouseButtonDown(0)){
                    insertText("J");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 247.5f-1f && selector.transform.eulerAngles.z < 247.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "K";
                if(Input.GetMouseButtonDown(0)){
                    insertText("K");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 236.25f-1f && selector.transform.eulerAngles.z < 236.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "L";
                if(Input.GetMouseButtonDown(0)){
                    insertText("L");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 225f-1f && selector.transform.eulerAngles.z < 225f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "M";
                if(Input.GetMouseButtonDown(0)){
                    insertText("M");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 213.75f-1f && selector.transform.eulerAngles.z < 213.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "N";
                if(Input.GetMouseButtonDown(0)){
                    insertText("N");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 202.5f-1f && selector.transform.eulerAngles.z < 202.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "O";
                if(Input.GetMouseButtonDown(0)){
                    insertText("O");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 191.25f-1f && selector.transform.eulerAngles.z < 191.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "P";
                if(Input.GetMouseButtonDown(0)){
                    insertText("P");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 180f-1f && selector.transform.eulerAngles.z < 180f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Q";
                if(Input.GetMouseButtonDown(0)){
                    insertText("Q");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 168.75f-1f && selector.transform.eulerAngles.z < 168.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "R";
                if(Input.GetMouseButtonDown(0)){
                    insertText("R");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 157.5f-1f && selector.transform.eulerAngles.z < 157.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "S";
                if(Input.GetMouseButtonDown(0)){
                    insertText("S");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 146.25f-1f && selector.transform.eulerAngles.z < 146f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "T";
                if(Input.GetMouseButtonDown(0)){
                    insertText("T");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 135f-1f && selector.transform.eulerAngles.z < 135f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "U";
                if(Input.GetMouseButtonDown(0)){
                    insertText("U");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 123.75f-1f && selector.transform.eulerAngles.z < 123.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "V";
                if(Input.GetMouseButtonDown(0)){
                    insertText("V");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 112.5f-1f && selector.transform.eulerAngles.z < 112.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "W";
                if(Input.GetMouseButtonDown(0)){
                    insertText("W");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 101.25f-1f && selector.transform.eulerAngles.z < 101.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "X";
                if(Input.GetMouseButtonDown(0)){
                    insertText("X");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 90f-1f && selector.transform.eulerAngles.z < 90f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Y";
                if(Input.GetMouseButtonDown(0)){
                    insertText("Y");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 78.75f-1f && selector.transform.eulerAngles.z < 78.75f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Z";
                if(Input.GetMouseButtonDown(0)){
                    insertText("Z");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 67.5f-1f && selector.transform.eulerAngles.z < 67.5f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Ä";
                if(Input.GetMouseButtonDown(0)){
                    insertText("Ä");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 56.25f-1f && selector.transform.eulerAngles.z < 56.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Ö";
                if(Input.GetMouseButtonDown(0)){
                    insertText("Ö");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 45f-1f && selector.transform.eulerAngles.z < 45f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "Ü";
                if(Input.GetMouseButtonDown(0)){
                    insertText("Ü");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 11.25f-1f && selector.transform.eulerAngles.z < 11.25f+1f){
                returnImage.SetActive(false);
                backspaceImage.SetActive(false);
                selectorText.text = "_";
                if(Input.GetMouseButtonDown(0)){
                    insertText(" ");
                    clickSound.Play();
                }
            } else if(selector.transform.eulerAngles.z >= 22.5f-1f && selector.transform.eulerAngles.z < 22.5f+1f){
                // Enter value
                selectorText.text = "";
                returnImage.SetActive(true);
                backspaceImage.SetActive(false);
                if(Input.GetMouseButtonDown(0)){
                    script.checkOutput(inputField.text);
                    inputField.text  = "";
                }
            } else if(selector.transform.eulerAngles.z >= 33.75f-1f && selector.transform.eulerAngles.z < 33.75f+1f){
                //backspace value
                selectorText.text = "";
                returnImage.SetActive(false);
                backspaceImage.SetActive(true);
                if(Input.GetMouseButtonDown(0)){
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
                }
            } 
        }
        
    }
}

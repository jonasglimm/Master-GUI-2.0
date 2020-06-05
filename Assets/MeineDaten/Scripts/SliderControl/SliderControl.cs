using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
public class SliderControl : MonoBehaviour
{
    private ValueControlCenter valueControlCenter;
    private StartSliderTask startSliderTask;
    private AudioSource clickSound;

    public TextMeshProUGUI zahlAufgabe;
    public TextMeshProUGUI anzahlFehler;
    public TextMeshProUGUI nummerDerAufgabe;
    public TextMeshProUGUI maxAnzahlAufgabe;
    public TextMeshProUGUI timeTextField;
    public GameObject panelCorrect;
    public GameObject panelWrong;
    public GameObject endPanel;

    public TextMeshProUGUI endOfScaleText;
    public TextMeshProUGUI handleNumber;
    public Slider valueSlider;

    private int aufgabenstellung;
    private int neueAufgabenstellung;
    private int fehlercounter;
    private int aufgabenNr;

    private int[] aufgabenListe = new int[15]; 

    private int endOfScale;
    private DateTime startTime;

    private Vector3 lastMouseCoordinate = Vector3.zero;
    // public Slider slider;
    private bool selected = false;
    public GameObject startPanel;
    public GameObject startPanelTouchscreen;

    public float swipeDistanceX;

    private void Awake() //intiate all variables which are set in different scripts
    {
        valueControlCenter = GameObject.Find("SliderControl").GetComponent<ValueControlCenter>();
        startSliderTask = GameObject.Find("SliderControl").GetComponent<StartSliderTask>();
        clickSound = GameObject.Find("ClickSound").GetComponent<AudioSource>();

        endOfScale = startSliderTask.endOfScale;
        CreateTaskOrder();
    }

    void Start()
    {
        fehlercounter = 0;
        aufgabenNr = 1;
        valueSlider.maxValue = endOfScale;
        valueSlider.value = endOfScale / 3;
        NewTask();
        SetStartPanel();

        if (valueControlCenter.touchpadInput == false){
            ColorBlock colorVar = valueSlider.colors;
            colorVar.selectedColor = new Color(1f, 0.8509804f, 0.4f, 1);
            valueSlider.colors = colorVar;
        }

        if (valueControlCenter.touchpadInput == true) // If the trackpad is used, the cursor will be reset to the middle of the screen each cursorResetTime - seconds
        {
            InvokeRepeating("CursorLock", valueControlCenter.cursorResetTime, valueControlCenter.cursorResetTime);
        }
    }

    void handleTouchpadInput(){
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        if(Input.GetMouseButtonDown(0) && selected == true)
        {
            //selected = false;
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
            Comparision();
            clickSound.Play();
            valueSlider.Select();
        }
        if (mouseDelta.x < -swipeDistanceX ){ // if difference less than zero, moved to left
            lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
            if(selected == true){ // checking if the mouse button is pressed down. 
                valueSlider.value--;
            }
        } else if(mouseDelta.x > swipeDistanceX ){ // if difference greater than zero, moved to right
            lastMouseCoordinate = Input.mousePosition;
            if(selected == true){
                valueSlider.value++;
            }
        } 
    }
    private void Update()
    {
        zahlAufgabe.text = aufgabenstellung.ToString();
        anzahlFehler.text = fehlercounter.ToString();
        nummerDerAufgabe.text = aufgabenNr.ToString();
        maxAnzahlAufgabe.text = valueControlCenter.numberOfTasks.ToString();

        endOfScaleText.text = endOfScale.ToString();
        handleNumber.text = valueSlider.value.ToString();
        if(valueControlCenter.touchpadInput == true){
            CursorUnlock();
            handleTouchpadInput();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartTime();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndScreen();
        }
    }

    public void SetStartPanel()
    {
        if (valueControlCenter.touchscreenInput == true)
        {
            startPanel.SetActive(false);
            startPanelTouchscreen.SetActive(true);
        }
        else
        {
            startPanelTouchscreen.SetActive(false);
            startPanel.SetActive(true);
        }
    }

    public void StartTime()
    {
        startTime = System.DateTime.Now;
        clickSound.Play();
        startPanel.SetActive(false);
        startPanelTouchscreen.SetActive(false);
        valueSlider.Select();
        selected = true;
    }

    public void Comparision()
    {
        if (valueSlider.value == aufgabenstellung){
            StartCoroutine(FeedbackCorrect());
            aufgabenNr++;
            if (aufgabenNr >= valueControlCenter.numberOfTasks){
                EndScreen();
            }
            NewTask();
        } else {
            fehlercounter++;
            StartCoroutine(FeedbackWrong());
        }

        IEnumerator FeedbackCorrect(){
            panelCorrect.SetActive(true);
            yield return new WaitForSecondsRealtime(valueControlCenter.feedbackPanelTime);
            panelCorrect.SetActive(false);
        }

        IEnumerator FeedbackWrong(){
            panelWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(valueControlCenter.feedbackPanelTime);
            panelWrong.SetActive(false);
        }
    }

    private void CreateTaskOrder() //Ramdom Order depending on the number of names
    {
        aufgabenListe[0] = endOfScale * 4 / 7;
        aufgabenListe[1] = endOfScale * 7 / 9;
        aufgabenListe[2] = endOfScale / 4 + 1;
        aufgabenListe[3] = endOfScale * 3/5;
        aufgabenListe[4] = endOfScale * 6/7;
        aufgabenListe[5] = endOfScale * 2/9;
        aufgabenListe[6] = endOfScale * 3 / 4;
        aufgabenListe[7] = endOfScale * 2 / 7;
        aufgabenListe[8] = endOfScale / 3;
        aufgabenListe[9] = endOfScale * 5/6;
        aufgabenListe[10] = endOfScale / 2;
        aufgabenListe[11] = endOfScale * 1/8;
        aufgabenListe[12] = endOfScale / 4;
        aufgabenListe[13] = endOfScale * 8/10;
        aufgabenListe[14] = endOfScale * 3/10;
    }

    private void NewTask()
    {
        int factor = aufgabenNr / aufgabenListe.Length; //start from the top of the list after counting through it
        if (aufgabenNr - (factor * aufgabenListe.Length) != 0)
        {
            if (aufgabenstellung != aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 1]) // prevent that the same name needs to be selected twice im a row
            {
                aufgabenstellung = aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 1];
            }
            else if (aufgabenstellung != aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 2]) // prevent that the same name needs to be selected twice im a row
            {
                aufgabenstellung = aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 2];
            }
            else if (aufgabenstellung != aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 3]) // prevent that the same name needs to be selected twice im a row
            {
                aufgabenstellung = aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 3];
            }
        }
        else
        {
            aufgabenstellung = aufgabenListe[aufgabenNr / factor - 1];
        }
    }


    public void EndScreen(){
        var totalTime = System.DateTime.Now - startTime;
        timeTextField.text = totalTime.Minutes.ToString()+" min : "+totalTime.Seconds.ToString() + " sec";
        endPanel.SetActive(true);
        CancelInvoke();
        selected = false;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class AuswahlControl : MonoBehaviour
{
    private AuswahlTrackpad auswahlTrackpad;
    private ValueControlCenter valueControlCenter;

    // Creating gameobjects to run the task and count the amount of wrong actions
    // Gameobject need to be assigned in the inspector in Unity
    public GameObject zahlAufgabe; 
    
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;

    // Different overlay panels to give visuell feedback
    public GameObject panelCorrect;
    public GameObject panelWrong;
    public GameObject endNachricht;
    public GameObject anzahlFehler;
    public TextMeshProUGUI timeTextField;
    public GameObject endPanel;
    public bool useIcons;

    // Private variables to use within the calculations
    private int aufgabenstellung;
    private int[] aufgabenListe = { 3, 1, 4, 2, 5, 1, 6, 4, 1, 5, 4, 2, 4, 6, 1, 3, 1, 4, 2, 5, 1, 6, 4, 1, 5, 4, 2, 4, 6, 1 };
    private int fehlercounter;
    private int aufgabenNr;

    private Button[] buttonList;
    private GameObject[] numberList;
    private GameObject[] iconList;
    private GameObject images;
  

    // Active time is the time in sec how long a feedback panel is shown
    private float activeTime; //set in ValueControlCenter
    // Number of tasks 
    private int anzahlAufgaben; //set in ValueControlCenter
    // Different start set up if the task show be done using direct touch
    private bool directTouchInput; //set in ValueControlCenter
    // If no direct touch is used, startButton is the first button to be highlighted
    private Button startButton; //set in ValueControlCenter
    private DateTime startTime;
    private GameObject startPanel;
    private GameObject startPanelTouchscreen;
    private AudioSource clickSound;

    void Awake()
    {
        valueControlCenter = GameObject.Find("AufgabenManager").GetComponent<ValueControlCenter>();
        auswahlTrackpad = GameObject.Find("AufgabenManager").GetComponent<AuswahlTrackpad>();
        buttonList = FindObjectsOfType<Button>();
        images = GameObject.Find("Images");
        startPanel = GameObject.Find("StartPanel");
        startPanelTouchscreen = GameObject.Find("StartPanelForTouchscreen");
        clickSound = GameObject.Find("AufgabenManager").GetComponent<AudioSource>();

        activeTime = valueControlCenter.feedbackPanelTime;
        anzahlAufgaben = valueControlCenter.numberOfTasks;
        directTouchInput = valueControlCenter.touchscreenInput;
        startButton = valueControlCenter.startButton;
    }

    void Start()
    {
        aufgabenNr = 1;
        ChangeToIcons();
        NewTask(); //Which Button should be pressed?
        // Starting to count mistakes and tasks
        fehlercounter = 0;

        // if direct touch is used, the selected color is changed to blue
        if (directTouchInput == true) 
         {
             for (var i = 0; i < buttonList.Length - 1; i++ ) // -1 because of backbutton
             {
                 ColorBlock colorVar = buttonList[i].colors;
                 colorVar.selectedColor = new Color(0.2666667f, 0.4470588f, 0.7686275f, 1);
                 buttonList[i].colors = colorVar;
                startPanel.SetActive(false);
             }
         }
        else
        {
            startPanelTouchscreen.SetActive(false);
        }
    }

    //The correct text is assigned to the different textelements shown on the Canvas
    private void Update() 
    {
        zahlAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = aufgabenstellung.ToString();
        anzahlFehler.GetComponent<TMPro.TextMeshProUGUI>().text = fehlercounter.ToString();
        nummerDerAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = anzahlAufgaben.ToString();

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartTime();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndScreen();
        }
    }

    public void StartTime()
    {
        startTime = System.DateTime.Now;
        clickSound.Play();
        startPanel.SetActive(false);
        startPanelTouchscreen.SetActive(false);
    }

    private void ChangeToIcons()
    {
        numberList = GameObject.FindGameObjectsWithTag("NoIcons");
        iconList = GameObject.FindGameObjectsWithTag("WithIcons");

        if (useIcons == true)
        {
            zahlAufgabe.GetComponent<TextMeshProUGUI>().enabled = false;

            for (var i = 0; i < numberList.Length; i++)
            {
                numberList[i].GetComponent<TextMeshProUGUI>().enabled = false;
                iconList[i].GetComponent<Image>().enabled = true;
            }
        }
        else
        {
            zahlAufgabe.GetComponent<TextMeshProUGUI>().enabled = true;

            for (var i = 0; i < numberList.Length; i++)
            {
                numberList[i].GetComponent<TextMeshProUGUI>().enabled = true;
                iconList[i].GetComponent<Image>().enabled = false;
                images.transform.GetChild(2).GetComponent<Image>().enabled = false;
            }
        }
    }

    // Function to be assigned to each button (OnButtonClicked) - compares the name (number) of the button to the current task
    // Feedback is given and either the task counter or the mistake counter is increased
    public void Comparision(Button btn)
    {

        if (btn.name == aufgabenstellung.ToString())
        {
            StartCoroutine(FeedbackCorrect());

            if (useIcons == true)
            {
                images.transform.GetChild(aufgabenstellung - 1).GetComponent<Image>().enabled = false;
            }

            aufgabenNr++;

            if(aufgabenNr >= anzahlAufgaben) // if task counter reaches the max number of task, the endscreem is called
            {
                EndScreen();
            }

            NewTask();
        }

        else
        { 
            fehlercounter++;
            StartCoroutine(FeedbackWrong());
        }

        IEnumerator FeedbackCorrect() // Correct Feedback for the time of activeTime
        {
            panelCorrect.SetActive(true);
            yield return new WaitForSecondsRealtime(activeTime);
            panelCorrect.SetActive(false);
        }

        IEnumerator FeedbackWrong() // Wrong Feedback for the time of activeTime
        {
            panelWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(activeTime);
            panelWrong.SetActive(false);
        }
    }

    private void NewTask() //Declare next Button to be pressed
    {
        int factor = aufgabenNr / aufgabenListe.Length;
        aufgabenstellung = aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 1];

        if (useIcons == true)
        {
            images.transform.GetChild(aufgabenstellung - 1).GetComponent<Image>().enabled = true;
        }
    }

    //Endscreen to show the endpanel
    public void EndScreen() {
        var totalTime = System.DateTime.Now - startTime;
        timeTextField.text = totalTime.Minutes.ToString()+" min : "+totalTime.Seconds.ToString() + " sek";
        endPanel.SetActive(true);
        endNachricht.SetActive(true);

        if (valueControlCenter.touchpadInput == true)
        {
            auswahlTrackpad.CancelInvoke();
            ShowCursor();
        }
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class BlaetterControl : MonoBehaviour
{
    private ButtonListBlaettern buttonListBlaettern;
    private ValueControlCenter valueControlCenter;
    private BlaetterRectMovement blaetterRectMovement;
    private AudioSource clickSound;

    // Gameobjects need to be assigned in the inspector in Unity
    public TextMeshProUGUI firstButtonText;

    // Creating gameobjects to run the task and count the amount of wrong actions
    public GameObject nameAufgabe;
    public GameObject anzahlFehler;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;

    // Different overlay panels to give visuell feedback
    public GameObject panelCorrect;
    public GameObject panelWrong;
    public GameObject endPanel;
    public TextMeshProUGUI timeTextField;
    // Private variables to use within the calculations
    private string gesuchteSeite;
    private int seitenzahl;
    private int[] seitenzahlListe = new int[15];

    private int fehlercounter;
    private int aufgabenNr;
    private int pagesLength;

    // Active time is the time in sec how long a feedback panel is shown
    private float activeTime;
    // Number of tasks 
    private int anzahlAufgaben;
    private DateTime startTime;
    private GameObject startPanel;
    private GameObject startPanelTouchscreen;

    private void Awake()
    {
        valueControlCenter = GameObject.Find("BlaetterManager").GetComponent<ValueControlCenter>();
        clickSound = GameObject.Find("BlaetterManager").GetComponent<AudioSource>();
        blaetterRectMovement = GameObject.Find("SnapOnScroll").GetComponent<BlaetterRectMovement>();
        buttonListBlaettern = GameObject.Find("BlaetterManager").GetComponent<ButtonListBlaettern>();
        startPanel = GameObject.Find("StartPanel");
        startPanelTouchscreen = GameObject.Find("StartPanelForTouchscreen");

        activeTime = valueControlCenter.feedbackPanelTime;
        anzahlAufgaben = valueControlCenter.numberOfTasks;
    }

    void Start()
    {
        // Starting to count mistakes and tasks
        fehlercounter = 0;
        aufgabenNr = 1;
        pagesLength = buttonListBlaettern.pages.Length;
        CreateTaskOrder();
        SetStartPanel();
        SetGesuchteSeite();
    }

    //The correct text is assigned to the different textelements shown on the Canvas
    private void Update()
    {
        nameAufgabe.GetComponent<TextMeshProUGUI>().text = gesuchteSeite;
        anzahlFehler.GetComponent<TextMeshProUGUI>().text = fehlercounter.ToString();
        nummerDerAufgabe.GetComponent<TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TextMeshProUGUI>().text = anzahlAufgaben.ToString();
        if (valueControlCenter.touchpadInput == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Comparision(blaetterRectMovement.buttonText[0]);
                blaetterRectMovement.selectedButton.Select();
                clickSound.Play();
            }
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
        }
        else
        {
            startPanelTouchscreen.SetActive(false);
        }
    }

    public void StartTime()
    {
        startTime = System.DateTime.Now;
        clickSound.Play();
        startPanel.SetActive(false);
        startPanelTouchscreen.SetActive(false);
    }

    // Function to be assigned to each button (OnButtonClicked) - compares the name (number) of the button to the current task
    // Feedback is given and either the task counter or the mistake counter is increased
    public void Comparision(TextMeshProUGUI buttonText)
    {
        if (buttonText.text == gesuchteSeite)
        {
            aufgabenNr++;
            StartCoroutine(FeedbackCorrect());
            SetGesuchteSeite();
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

            if (aufgabenNr >= anzahlAufgaben) // if task counter reaches the max number of task, the endscreem is called
            {
                EndScreen();
            }
        }

        IEnumerator FeedbackWrong() // Wrong Feedback for the time of activeTime
        {
            panelWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(activeTime);
            panelWrong.SetActive(false);
        }
    }

    private void CreateTaskOrder() //Ramdom Order depending on the number of pages
    {
        seitenzahlListe[0] = pagesLength / 2 + 1 ;
        seitenzahlListe[1] = pagesLength - 2;
        seitenzahlListe[2] = pagesLength / 4 + 1;
        seitenzahlListe[3] = 1;
        seitenzahlListe[4] = pagesLength - 1;
        seitenzahlListe[5] = pagesLength - (pagesLength/4);
        seitenzahlListe[6] = pagesLength;
        seitenzahlListe[7] = 2;
        seitenzahlListe[8] = pagesLength / 3;
        seitenzahlListe[9] = pagesLength - 2;
        seitenzahlListe[10] = pagesLength / 2;
        seitenzahlListe[11] = pagesLength / 2 + 2;
        seitenzahlListe[12] = pagesLength/4;
        seitenzahlListe[13] = pagesLength * 5/6;
        seitenzahlListe[14] = pagesLength - 1;
    }

    // functions to select a random page as the new button to press
    public void SetGesuchteSeite()
    {
        int factor = aufgabenNr / seitenzahlListe.Length; //start from the top of the list after counting through it
        if (aufgabenNr - (factor * seitenzahlListe.Length) != 0)
        {
            if (seitenzahl != seitenzahlListe[aufgabenNr - (factor * seitenzahlListe.Length) - 1]) // prevent that the same button (page) need to be selected twice im a row
            {
                seitenzahl = seitenzahlListe[aufgabenNr - (factor * seitenzahlListe.Length) - 1];
            }
            else if (seitenzahl != seitenzahlListe[aufgabenNr - (factor * seitenzahlListe.Length) - 2]) // prevent that the same button (page) need to be selected twice im a row
            {
                seitenzahl = seitenzahlListe[aufgabenNr - (factor * seitenzahlListe.Length) - 2];
            }
            else if (seitenzahl != seitenzahlListe[aufgabenNr - (factor * seitenzahlListe.Length) - 3]) // prevent that the same button (page) need to be selected twice im a row
            {
                seitenzahl = seitenzahlListe[aufgabenNr - (factor * seitenzahlListe.Length) - 3];
            }
        }
        else
        {
            seitenzahl = seitenzahlListe[aufgabenNr / factor - 1];
        }

        if (seitenzahl == pagesLength) // the first Button is not included within the array and needs to be included like this
            {
                gesuchteSeite = firstButtonText.text;
            }
        else
            {
                gesuchteSeite = buttonListBlaettern.pages[seitenzahl];
            }
    }

    //activates the endpanel
    public void EndScreen()
    {
        var totalTime = System.DateTime.Now - startTime;
        timeTextField.text = totalTime.Minutes.ToString()+" min : "+totalTime.Seconds.ToString() + " sek";
        endPanel.SetActive(true);

        if (valueControlCenter.touchpadInput == true)
        {
            blaetterRectMovement.CancelInvoke();
            ShowCursor();
        }
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
    }
}

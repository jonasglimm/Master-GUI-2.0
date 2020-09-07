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
    private ScrollSnapRect scrollSnapRect;
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
    private bool trackpadWasClicked;

    // Active time is the time in sec how long a feedback panel is shown
    private float activeTime;
    // Number of tasks 
    private int anzahlAufgaben;
    private DateTime startTime;
    public GameObject startPanel;
    public GameObject startPanelTouchscreen;

    //For iDriveController
    private IDriveController iDriveController;

    private void Awake() //assign values and functions to local variables 
    {
        valueControlCenter = GameObject.Find("BlaetterManager").GetComponent<ValueControlCenter>();
        clickSound = GameObject.Find("BlaetterManager").GetComponent<AudioSource>();
        blaetterRectMovement = GameObject.Find("SnapOnScroll").GetComponent<BlaetterRectMovement>();
        scrollSnapRect = GameObject.Find("SnapOnScroll").GetComponent<ScrollSnapRect>();
        buttonListBlaettern = GameObject.Find("BlaetterManager").GetComponent<ButtonListBlaettern>();
        iDriveController = GameObject.Find("BlaetterManager").GetComponent<IDriveController>();

        activeTime = valueControlCenter.feedbackPanelTime;
        anzahlAufgaben = valueControlCenter.numberOfTasks;
    }

    void Start()
    {
        // Starting to count mistakes and tasks
        fehlercounter = 0;
        aufgabenNr = 1;
        pagesLength = buttonListBlaettern.pages.Length; //count how many pages are being created
        CreateTaskOrder();
        SetStartPanel();
        SetGesuchteSeite();
    }

    //The correct text is assigned to the different textelements shown on the Canvas
    private void Update()
    {
        //update the displayes GUI-elements each frame
        nameAufgabe.GetComponent<TextMeshProUGUI>().text = gesuchteSeite;
        anzahlFehler.GetComponent<TextMeshProUGUI>().text = fehlercounter.ToString();
        nummerDerAufgabe.GetComponent<TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TextMeshProUGUI>().text = anzahlAufgaben.ToString();

        if (valueControlCenter.touchpadInput == true)
        {
            if (Input.GetMouseButtonDown(0)) //check the selected page/button if the touchpad is clicked
            {
                Comparision(blaetterRectMovement.buttonText[0]);
                blaetterRectMovement.selectedButton.Select();
                clickSound.Play();
            }
        }

        if (valueControlCenter.iDriveInput)
        {
            if (iDriveController.pushedOnce) //check the selected page/button is if the iDrive-Controller is clicked
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

    public void SetStartPanel() //initiate the start panel for either touchscreen or touchpad/iDrive-Controller 
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

    public void StartTime() //register the current time (for measuring ToT) and deactivate the start panel
    {
        startTime = System.DateTime.Now;
        clickSound.Play();
        startPanel.SetActive(false);
        startPanelTouchscreen.SetActive(false);

        if (valueControlCenter.touchscreenInput)
        {
            scrollSnapRect.buttons[scrollSnapRect.startingPage].Select();
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class ScrollTaskControl : MonoBehaviour
{
    //references to different scripts
    public ButtonListControl buttonListControl;
    private ScrollRectMovement scrollRectMovement;
    private ValueControlCenter valueControlCenter;
    private IDriveController iDriveController;
    //GUI elements
    public TextMeshProUGUI timeTextField;
    public GameObject nameAufgabe;
    public GameObject anzahlFehler;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;

    public GameObject panelCorrect;
    public GameObject panelWrong;
    public GameObject endNachricht;
    public GameObject endPanel;

    public AudioSource clickSound;

    //private variables for internal calculations
    private string gesuchterName;
    private int listenEintrag;
    private int[] aufgabenListe = new int[15];

    private int fehlercounter;
    private int aufgabenNr;
    private int namesLength;

    private float activeTime;
    private int anzahlAufgaben;
    private DateTime startTime;
    public GameObject startPanel;
    public GameObject startPanelTouchscreen;

    private void Awake() //assign local variables to the scripts
    {
        valueControlCenter = GameObject.Find("ScrollManager").GetComponent<ValueControlCenter>();
        scrollRectMovement = GameObject.Find("ButtonScrollList").GetComponent<ScrollRectMovement>();
        iDriveController = GameObject.Find("ScrollManager").GetComponent<IDriveController>();
    }

    void Start() 
    {
        //set variables 
        activeTime = valueControlCenter.feedbackPanelTime;
        anzahlAufgaben = valueControlCenter.numberOfTasks;
        namesLength = buttonListControl.names.Length;
        fehlercounter = 0;
        aufgabenNr = 1;
        //create tasks
        CreateTaskOrder();
        SetStartPanel();
        NewTask();

        if (!valueControlCenter.iDriveInput) //deactivate iDriveInputs to avoid unwanted interference
        {
            iDriveController.enabled = false;
        }
    }

    private void Update()
    {
        //update the GUI elements
        nameAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = gesuchterName;
        anzahlFehler.GetComponent<TMPro.TextMeshProUGUI>().text = fehlercounter.ToString();
        nummerDerAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = anzahlAufgaben.ToString();

        if (valueControlCenter.touchpadInput == true)
        {
            //start the comparison if the mouse press/touchpad is tapped/pressed
            if (Input.GetMouseButtonDown(0)){
                Comparision(scrollRectMovement.buttonText[0]);
                scrollRectMovement.selectedButton.Select();
                clickSound.Play();
            }
        }

        if (valueControlCenter.iDriveInput)
        {
            //start the comparison if the iDrive-Controller is pressed
            if (iDriveController.pushedOnce)
            {
                Comparision(scrollRectMovement.buttonText[0]);
                scrollRectMovement.selectedButton.Select();
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

    public void SetStartPanel() //activate the start panel depending if the modality is touchscreen input 
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

    public void StartTime() //start the timer for ToT
    {
        startTime = System.DateTime.Now;
        clickSound.Play();
        startPanel.SetActive(false);
        startPanelTouchscreen.SetActive(false);
    }

    public void Comparision(TextMeshProUGUI buttonText) //compare if the selected name is correct
    {
        if (buttonText.text == gesuchterName)
        {
            aufgabenNr++;
            StartCoroutine(FeedbackCorrect());
            NewTask();
        }

        else
        {
            fehlercounter++;
            StartCoroutine(FeedbackWrong());
        }
    }

    private void NewTask()
    {
        int factor = aufgabenNr / aufgabenListe.Length; //start from the top of the list after counting through it
        if (aufgabenNr - (factor * aufgabenListe.Length) != 0)
        {
            if (listenEintrag != aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 1]) // prevent that the same name needs to be selected twice im a row
            {
                listenEintrag = aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 1];
            }
            else if (listenEintrag != aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 2]) // prevent that the same name needs to be selected twice im a row
            {
                listenEintrag = aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 2];
            }
            else if (listenEintrag != aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 3]) // prevent that the same name needs to be selected twice im a row
            {
                listenEintrag = aufgabenListe[aufgabenNr - (factor * aufgabenListe.Length) - 3];
            }
        }
        else
        {
            listenEintrag = aufgabenListe[aufgabenNr / factor - 1];
        }
        gesuchterName = buttonListControl.names[listenEintrag];
    }

    private void CreateTaskOrder() //Ramdom order depending on the number of names
    {
        aufgabenListe[0] = namesLength / 2 + 1;
        aufgabenListe[1] = namesLength - 2;
        aufgabenListe[2] = namesLength / 4 + 1;
        aufgabenListe[3] = 1;
        aufgabenListe[4] = namesLength - 1;
        aufgabenListe[5] = namesLength * 2/9;
        aufgabenListe[6] = namesLength * 3/4;
        aufgabenListe[7] = 2;
        aufgabenListe[8] = namesLength / 3;
        aufgabenListe[9] = namesLength - 3;
        aufgabenListe[10] = namesLength / 2;
        aufgabenListe[11] = namesLength / 2 + 2;
        aufgabenListe[12] = namesLength / 4;
        aufgabenListe[13] = namesLength * 5/6;
        aufgabenListe[14] = namesLength - 1;
    }

    IEnumerator FeedbackCorrect()
        {
            panelCorrect.SetActive(true);
            yield return new WaitForSecondsRealtime(activeTime);
            panelCorrect.SetActive(false);

            if (aufgabenNr >= anzahlAufgaben)
            {
                EndScreen();
            }
        }

        IEnumerator FeedbackWrong()
        {
            panelWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(activeTime);
            panelWrong.SetActive(false);
        }

    public void EndScreen()
    {
        var totalTime = System.DateTime.Now - startTime;
        timeTextField.text = totalTime.Minutes.ToString()+" min : "+totalTime.Seconds.ToString() + " sec";
        endPanel.SetActive(true);
        endNachricht.SetActive(true);
        Cursor.visible = true;
    }

}

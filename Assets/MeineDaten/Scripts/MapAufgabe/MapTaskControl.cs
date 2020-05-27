using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MapTaskControl : MonoBehaviour
{ 

    public GameObject pinNorth;
    public GameObject pinSouth;
    public GameObject pinWest;
    public GameObject pinEast;

    public GameObject pointer;

    public GameObject nameAufgabe;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;

    public GameObject panelCorrect;
    public GameObject endPanel;

    private bool taskNorth;
    private bool taskSouth;
    private bool taskEast;
    private bool taskWest;

    private string gesuchteSeite;

    private int aufgabenNr;
    private string gesuchteMarkierung;

    private ValueControlCenter valueControlCenter;
    private MapControl mapControl;
    private AudioSource clickSound;

    private void Awake()
    {
        valueControlCenter = GameObject.Find("MapManager").GetComponent<ValueControlCenter>();
        mapControl = GameObject.Find("MapManager").GetComponent<MapControl>();
        clickSound = GameObject.Find("MapManager").GetComponent<AudioSource>();
    }

    void Start()
    {
        pinNorth.SetActive(true);
        pinSouth.SetActive(false);
        pinWest.SetActive(false);
        pinEast.SetActive(false);

        taskNorth = true;
        taskSouth = taskEast = taskWest = false;
        aufgabenNr = 1;
        gesuchteMarkierung = "Markierung im Norden!";
    }

    void Update()
    {
        nameAufgabe.GetComponent<TextMeshProUGUI>().text = gesuchteMarkierung;
        nummerDerAufgabe.GetComponent<TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TextMeshProUGUI>().text = valueControlCenter.numberOfTasks.ToString();

        if (taskNorth == true && pinNorth.GetComponent<PinNorthCollider>().pinNorthEntered == true)
        {
            StartCoroutine(FeedbackCorrect());
            clickSound.Play();
            taskNorth = false;
            pinNorth.SetActive(false);
            taskEast = true;
            pinEast.SetActive(true);

            gesuchteMarkierung = "Markierung im Osten!";
            aufgabenNr++;
        }

        if (taskEast == true && pinEast.GetComponent<PinEastCollider>().pinEastEntered == true)
        {
            StartCoroutine(FeedbackCorrect());
            clickSound.Play();
            taskEast = false;
            pinEast.SetActive(false);
            taskWest = true;
            pinWest.SetActive(true);

            gesuchteMarkierung = "Markierung im Westen!";
            aufgabenNr++;
        }

        if (taskWest == true && pinWest.GetComponent<PinWestCollider>().pinWestEntered == true)
        {
            StartCoroutine(FeedbackCorrect());
            clickSound.Play();
            taskWest = false;
            pinWest.SetActive(false);
            taskSouth = true;
            pinSouth.SetActive(true);

            gesuchteMarkierung = "Markierung im Süden!";
            aufgabenNr++;
        }

        if (taskSouth == true && pinSouth.GetComponent<PinSouthCollider>().pinSouthEntered == true)
        {
           StartCoroutine(FeedbackCorrect());
            clickSound.Play();
            taskSouth = false;
            pinSouth.SetActive(false);

            EndScreen();
        }
    }


    IEnumerator FeedbackCorrect()
    {
        panelCorrect.SetActive(true);
        yield return new WaitForSecondsRealtime(valueControlCenter.feedbackPanelTime);
        panelCorrect.SetActive(false);
    } 

    public void EndScreen()
    {
        pointer.SetActive(false);
        endPanel.SetActive(true);

        if(valueControlCenter.touchpadInput == true)
        {
            mapControl.CancelInvoke();
            ShowCursor();
        }
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
    }
}

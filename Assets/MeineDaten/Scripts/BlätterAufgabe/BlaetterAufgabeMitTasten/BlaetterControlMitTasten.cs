using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BlaetterControlMitTasten : MonoBehaviour
{
    /// <summary>
    /// Basicly the same scripts as BlaetterControl just with a different class for the button list
    /// </summary>

    public ButtonListBlaetternMitTasten buttonListBlaettern; //only difference: this class needs to be called
    public BlaetterRectMovement blaetterRectMovement;
    public TextMeshProUGUI firstButtonText;

    public GameObject nameAufgabe;
    public GameObject anzahlFehler;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;

    public GameObject panelCorrect;
    public GameObject panelWrong;
    public GameObject endPanel;

    private string gesuchteSeite;
    private string neueSeite;
    private int seitenzahl;

    private int fehlercounter;
    private int aufgabenNr;
    private int pagesLength;

    private float activeTime = 0.5f; //set in ValueControlCenter
    private int anzahlAufgaben = 5; //set in ValueControlCenter

    [Header("All values set in:")]
    public ValueControlCenter valueControlCenter;

    public AudioSource clickSound;

    private void Awake()
    {
        activeTime = valueControlCenter.feedbackPanelTime;
        anzahlAufgaben = valueControlCenter.numberOfTasks;
    }

    void Start()
    {
        pagesLength = buttonListBlaettern.pages.Length;
        SetGesuchteSeite();
        fehlercounter = 0;
        aufgabenNr = 1;
    }

    private void Update()
    {
        nameAufgabe.GetComponent<TextMeshProUGUI>().text = gesuchteSeite;
        anzahlFehler.GetComponent<TextMeshProUGUI>().text = fehlercounter.ToString();
        nummerDerAufgabe.GetComponent<TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TextMeshProUGUI>().text = anzahlAufgaben.ToString();
        if(valueControlCenter.touchpadInput == true){
            if(Input.GetMouseButtonDown(0)){
                Comparision(blaetterRectMovement.buttonText[0]);
                blaetterRectMovement.selectedButton.Select();
                clickSound.Play();
            }
        }
        
    }

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

    }
    public void SetGesuchteSeite()
    {
        gesuchteSeite = neueSeite;
        while (neueSeite == gesuchteSeite)
        {
            seitenzahl = Random.Range(0, pagesLength); 

            if (seitenzahl == pagesLength) // because then outside bounds of array and first Button needs to be included
            {
                neueSeite = firstButtonText.text;
            }
            else
            {
                neueSeite = buttonListBlaettern.pages[seitenzahl];
            }
        }
        gesuchteSeite = neueSeite;
    }

    public void EndScreen()
    {
        endPanel.SetActive(true);
    }

    

}

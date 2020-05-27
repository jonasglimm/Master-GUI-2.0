using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollTaskControlMitTasten : MonoBehaviour
{
    public ButtonListControlMitTasten buttonListControl;
    public ScrollRectMovement scrollRectMovement;

    public GameObject nameAufgabe;
    public GameObject anzahlFehler;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;

    public GameObject panelCorrect;
    public GameObject panelWrong;
    public GameObject endNachricht;
    public GameObject endPanel;
    private string gesuchterName;
    private string neuerName;

    private int fehlercounter;
    private int aufgabenNr;
    private int namesLength; 

    public float activeTime = 0.5f;
    public int anzahlAufgaben = 5;

    void Start()
    {
        namesLength = buttonListControl.names.Length;
        gesuchterName = buttonListControl.names[Random.Range(0, namesLength)];
        fehlercounter = 0;
        aufgabenNr = 1;
    }

    private void Update()
    {
        nameAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = gesuchterName;
        anzahlFehler.GetComponent<TMPro.TextMeshProUGUI>().text = fehlercounter.ToString();
        nummerDerAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = anzahlAufgaben.ToString();
        if(Input.GetMouseButtonDown(0)){
            Comparision(scrollRectMovement.buttonText[0]);
            scrollRectMovement.selectedButton.Select();
        }
    }

    public void Comparision(TextMeshProUGUI buttonText)
    {
        if (buttonText.text == gesuchterName)
        {
            aufgabenNr++;
            StartCoroutine(FeedbackCorrect());

            neuerName = buttonListControl.names[Random.Range(0, namesLength)];

            while (neuerName == gesuchterName)
            {
                neuerName = buttonListControl.names[Random.Range(0, namesLength)];
            }

            gesuchterName = neuerName;
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
    public void EndScreen()
    {
        endPanel.SetActive(true);
        endNachricht.SetActive(true);
    }

}

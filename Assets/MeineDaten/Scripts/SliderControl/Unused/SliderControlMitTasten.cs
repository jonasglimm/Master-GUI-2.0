using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SliderControlMitTasten : MonoBehaviour
{
    public TextMeshProUGUI zahlAufgabe;
    public TextMeshProUGUI anzahlFehler;
    public TextMeshProUGUI nummerDerAufgabe;
    public TextMeshProUGUI maxAnzahlAufgabe;

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

    public float feedbackTime;
    public int anzahlAufgaben;
    public int endOfScale;

    void Start()
    {
        valueSlider.maxValue = endOfScale;
        valueSlider.value = endOfScale / 2;
        NeueAufgabenstellung();
        fehlercounter = 0;
        aufgabenNr = 1;
    }

    private void Update()
    {
        zahlAufgabe.text = aufgabenstellung.ToString();
        anzahlFehler.text = fehlercounter.ToString();
        nummerDerAufgabe.text = aufgabenNr.ToString();
        maxAnzahlAufgabe.text = anzahlAufgaben.ToString();

        endOfScaleText.text = endOfScale.ToString();
        handleNumber.text = valueSlider.value.ToString();

    }

    public void Comparision()
    {
            if (valueSlider.value == aufgabenstellung)
            {
                StartCoroutine(FeedbackCorrect());

                aufgabenNr++;
                if (aufgabenNr >= anzahlAufgaben)
                {
                    EndScreen();
                }

                NeueAufgabenstellung();
            }

            else
            {
                fehlercounter++;
                StartCoroutine(FeedbackWrong());
            }

        IEnumerator FeedbackCorrect()
        {
            panelCorrect.SetActive(true);
            yield return new WaitForSecondsRealtime(feedbackTime);
            panelCorrect.SetActive(false);
        }

        IEnumerator FeedbackWrong()
        {
            panelWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(feedbackTime);
            panelWrong.SetActive(false);
        }
    }
    public void NeueAufgabenstellung()
    {
        neueAufgabenstellung = Random.Range(0, endOfScale + 1);

        while (neueAufgabenstellung == aufgabenstellung)
        {
            neueAufgabenstellung = Random.Range(0, endOfScale +1);
        }

        aufgabenstellung = neueAufgabenstellung;
    }

    public void EndScreen()
    {
        endPanel.SetActive(true);
    }

    
}

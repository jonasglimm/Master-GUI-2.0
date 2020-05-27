using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickFeedback : MonoBehaviour
{
   // private bool mFaded = false;

    //public float duration = 0.4f;
    public GameObject panelCorrect;
    public GameObject panelWrong;

    public void Start()
    {
        StartCoroutine(PanelCorrectHighlight());
        StartCoroutine(PanelWrongHighlight());
    }

    /*
    public IEnumerator FadeCorrect()
    {
        float counter = 0f;
        panelCorrect.SetActive(true);

        
        while (counter < duration)
        {
            counter += Time.deltaTime;
            panelCorrect.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0.0f, 1.0f, counter / duration);

            yield return null;
        }


      

        
        counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            panelCorrect.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, counter / duration);

            yield return null;
        }
        

        panelCorrect.SetActive(false);

    }


        public IEnumerator FadeWrong()
        {
            float counter = 0f;
        panelWrong.SetActive(true);

            while (counter < duration)
            {
                counter += Time.deltaTime;
                panelWrong.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0.0f, 1.0f, counter / duration);

                yield return null;
            }


        //new WaitForSeconds(0.5f);

        counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            panelWrong.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, counter / duration);

            yield return null;
        }

        panelWrong.SetActive(false);
    }

    */

    public IEnumerator PanelCorrectHighlight()
    {
        Debug.Log("Ab jetzt warten");
        panelCorrect.SetActive(true);
        yield return new WaitForSeconds(3f);
        panelCorrect.SetActive(false);
        Debug.Log("Fertig gewartet");
    }

    public IEnumerator PanelWrongHighlight()
    {
        Debug.Log("Ab jetzt warten");
        panelWrong.SetActive(true);
        yield return new WaitForSeconds(3f);
        panelWrong.SetActive(false);
        Debug.Log("Fertig gewartet");
    }
}

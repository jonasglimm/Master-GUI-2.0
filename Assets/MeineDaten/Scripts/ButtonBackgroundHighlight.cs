using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBackgroundHighlight : MonoBehaviour, /*IPointerEnterHandler,*/ ISelectHandler
{
    public GameObject feedback;
    //public Button thisButton;


    public void Update()
    {
      // if (thisButton.OnDeselect == true)
        //{

        //}
    }

    public void OnSelect(BaseEventData eventData)
    {
        // feedback.SetActive(true);
    }

    public void OnDeselect (BaseEventData eventData)
    {
        // feedback.SetActive(false);
    }
    
    /*
    public void OnPointerEnter (PointerEventData eventData)
    {
        feedback.SetActive(true);
    }

 public void OnPointerExit (PointerEventData eventData)
    {
        feedback.SetActive(false);
    }
    */
}

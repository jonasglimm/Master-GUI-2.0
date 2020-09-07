using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChangeButtonTextColor : MonoBehaviour //, ISelectHandler
{
    public ValueControlCenter valueControlCenter;
    public GameObject button;

    /*
    public void OnSelect(BaseEventData eventData)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        Debug.Log("Select");
    }
    */
    /*
    public void OnDeselect(BaseEventData eventData)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        Debug.Log("Deselect");
    }*/

    
    // Update is called once per frame
    void Update()
    {
        if (!valueControlCenter.touchscreenInput)
        {
            if (EventSystem.current.currentSelectedGameObject == button) //if the button is selected, it will be yellow -> text must be black
            {
                button.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            }
            else
            {
                button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            }
        }
    }
    
}

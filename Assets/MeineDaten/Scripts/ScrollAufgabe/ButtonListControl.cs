using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonListControl : MonoBehaviour
{
    public GameObject buttonTemplate;
    private ScrollRectMovement scrollRectMovement;
    private ValueControlCenter valueControlCenter;
    //private ButtonSelectionController buttonSelectionController;

    public string[] names; //list of all possible names

    private void Awake()
    {
        //assign scripts to the local variables
        scrollRectMovement = GameObject.Find("ButtonScrollList").GetComponent<ScrollRectMovement>();
        valueControlCenter = GameObject.Find("ScrollManager").GetComponent<ValueControlCenter>();
        //buttonSelectionController = GameObject.Find("ButtonScrollList").GetComponent<ButtonSelectionController>();
    }

    private void Start()
    {
        if (valueControlCenter.touchscreenInput)
        {
            //disable the yellow cursor for touchscreeninput
            ColorBlock colorVar = buttonTemplate.GetComponent<Button>().colors;
            colorVar.selectedColor = new Color(0.2666667f, 0.4470588f, 0.7686275f, 1);
            buttonTemplate.GetComponent<Button>().colors = colorVar;
        }

        for (int i = 0; i < names.Length; i++)
        {
            //initiate a button for each manually added name
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);
            button.GetComponent<ButtonListButton>().SetText(names[i]);
            button.transform.SetParent(buttonTemplate.transform.parent, false);

        }
        scrollRectMovement.SetUp();
        //buttonSelectionController.SetUp();
    }
}

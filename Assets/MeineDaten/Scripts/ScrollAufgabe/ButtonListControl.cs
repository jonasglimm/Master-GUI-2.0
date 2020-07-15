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

    public string[] names;

    private void Awake()
    {
        scrollRectMovement = GameObject.Find("ButtonScrollList").GetComponent<ScrollRectMovement>();
        valueControlCenter = GameObject.Find("ScrollManager").GetComponent<ValueControlCenter>();
        //buttonSelectionController = GameObject.Find("ButtonScrollList").GetComponent<ButtonSelectionController>();
    }

    private void Start()
    {
        if (valueControlCenter.touchscreenInput)
        {
            ColorBlock colorVar = buttonTemplate.GetComponent<Button>().colors;
            colorVar.selectedColor = new Color(0.2666667f, 0.4470588f, 0.7686275f, 1);
            buttonTemplate.GetComponent<Button>().colors = colorVar;
        }

        for (int i = 0; i < names.Length; i++)
        {
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);
            button.GetComponent<ButtonListButton>().SetText(names[i]);
            button.transform.SetParent(buttonTemplate.transform.parent, false);

        }
        scrollRectMovement.SetUp();
        //buttonSelectionController.SetUp();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonListControl : MonoBehaviour
{
    public GameObject buttonTemplate;
    private ScrollRectMovement scrollRectMovement;
    //private ButtonSelectionController buttonSelectionController;

    public string[] names;

    private void Awake()
    {
        scrollRectMovement = GameObject.Find("ButtonScrollList").GetComponent<ScrollRectMovement>();
        //buttonSelectionController = GameObject.Find("ButtonScrollList").GetComponent<ButtonSelectionController>();
    }

    private void Start()
    {
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

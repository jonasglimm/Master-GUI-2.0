using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonListControlMitTasten : MonoBehaviour
{
    public ButtonSelectionController buttonSelectionController;
    public ScrollRectMovement scrollRectMovement;
    public GameObject buttonTemplate;

    public string[] names;



    private void Start()
    {
        for (int i = 0; i < names.Length; i++)
        {

            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            button.GetComponent<ButtonListButton>().SetText(names[i]);

            button.transform.SetParent(buttonTemplate.transform.parent, false);

        }

        //buttonSelectionController.SetUp();
        scrollRectMovement.SetUp();
    }
  
}

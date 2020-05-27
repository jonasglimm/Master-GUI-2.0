using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonListBlaettern : MonoBehaviour
{
    // first button to set a template (needs to be assigned)
    public GameObject firstButton;
    public string firstButtonText; // because the first Button is used, the text of the first button can be changed

    // Array to set the amount of pages (+ first Button) and name each created Button
    public string[] pages;

    private ValueControlCenter valueControlCenter;
    private BlaetterRectMovement blaetterRectMovement;
    private PageSelectionCreator pageSelectionCreator;

    private void Start()
    {
        valueControlCenter = GameObject.Find("BlaetterManager").GetComponent<ValueControlCenter>();
        blaetterRectMovement = GameObject.Find("SnapOnScroll").GetComponent<BlaetterRectMovement>();
        pageSelectionCreator = GameObject.Find("BlaetterManager").GetComponent<PageSelectionCreator>();
        // Intiating a button for each element in the pages array
        for (int i = 0; i < pages.Length; i++)
        {
            GameObject button = Instantiate(firstButton) as GameObject;
            button.SetActive(true);
            button.GetComponent<ButtonListButtonBlaettern>().SetText(pages[i]);
            button.transform.SetParent(firstButton.transform.parent, false);
        }
        // Assigning the changable text of the first button
        firstButton.GetComponent<ButtonListButtonBlaettern>().SetText(firstButtonText);

        pageSelectionCreator.SetUp();

        if (valueControlCenter.touchscreenInput == false)
        {
            blaetterRectMovement.SetUp(); //setting up the movement of the scroll rect while using keys
        }

    }

}

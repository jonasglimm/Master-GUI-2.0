using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonListBlaetternMitTasten : MonoBehaviour
{
    //difference to ButtonListBlaettern is the setUp of blaetterRectMovement
    public BlaetterRectMovement blaetterRectMovement;

    public GameObject firstButton;

    public string firstButtonText;


    public string[] pages;


    private void Start()
    {
       
        for (int i = 0; i < pages.Length; i++)
        {
            GameObject button = Instantiate(firstButton) as GameObject;
            button.SetActive(true);

            button.GetComponent<ButtonListButtonBlaettern>().SetText(pages[i]);

            button.transform.SetParent(firstButton.transform.parent, false);

        }

        firstButton.GetComponent<ButtonListButtonBlaettern>().SetText(firstButtonText);
        blaetterRectMovement.SetUp(); //setting up the movement of the scroll rect while using keys

    }

}

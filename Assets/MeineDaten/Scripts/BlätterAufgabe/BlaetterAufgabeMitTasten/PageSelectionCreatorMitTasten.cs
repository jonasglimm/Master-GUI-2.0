using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSelectionCreatorMitTasten : MonoBehaviour
{
    public GameObject pageSelectionTemplate;

    public ButtonListBlaetternMitTasten buttonListBlaettern; // only difference to PageSelectionCreator


    private void Start()
    {
        for (int i = 0; i < buttonListBlaettern.pages.Length; i++)
        {

            GameObject pageSelection = Instantiate(pageSelectionTemplate) as GameObject;
            pageSelection.SetActive(true);

            pageSelection.transform.SetParent(pageSelectionTemplate.transform.parent, false);

        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to create the page selection icons at the below the different buttons
public class PageSelectionCreator : MonoBehaviour
{
    // Gameobject for the first icon as a template need to be assigned in inspector
    public GameObject pageSelectionTemplate;
    // A gameobject with this script attached needs to be assigned in the inspector
    public ButtonListBlaettern buttonListBlaettern;

    //Initiating as many page selection icons, as buttons are created
    public void SetUp()
    {
        for (int i = 0; i < buttonListBlaettern.pages.Length; i++)
        {
            GameObject pageSelection = Instantiate(pageSelectionTemplate) as GameObject;
            pageSelection.SetActive(true);

            pageSelection.transform.SetParent(pageSelectionTemplate.transform.parent, false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonListButton : MonoBehaviour
{

    public TextMeshProUGUI myText;

    //public ButtonListControl buttonControl;

    //private string myTextString;



    public void SetText(string textString)
    {
        //myTextString = textString;
        myText.text = textString;
    }

    /*
    public void OnClick()
    {
        buttonControl.ButtonClicked(myTextString);
    }*/
}

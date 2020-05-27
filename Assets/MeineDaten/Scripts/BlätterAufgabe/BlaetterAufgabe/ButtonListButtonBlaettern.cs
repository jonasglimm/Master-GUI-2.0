using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ButtonListButtonBlaettern : MonoBehaviour
{
    public TextMeshProUGUI myText;

    public void SetText(string textString)
    {
        myText.text = textString;
    }

}

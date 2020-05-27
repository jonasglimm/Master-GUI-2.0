using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonName : MonoBehaviour
{

    //public GameObject NumberAufgabenstellung = new GameObject();

    public void btnTest(Button btn)
    {
        //Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        Debug.Log(btn.name);

    }

    //string 

}

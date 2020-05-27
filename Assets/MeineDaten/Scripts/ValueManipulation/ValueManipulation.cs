using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ValueManipulation : MonoBehaviour
{
    [Header("GUI Elements")]
    public Image selector;
    public TextMeshProUGUI selectionValue;
    
    [Header("Sounds")]
    public AudioSource scrollSound;
    private int value;
    private ControlManager script;
    // Start is called before the first frame update
    void Start()
    {
        value = 0;
        selector.fillAmount = 0.10943f;
        selectionValue.text = value.ToString();
        script = gameObject.GetComponent<ControlManager>();
    }

    // Update is called once per frame
    void Update(){
        if(script.iDriveInput == true){
            if(Input.mouseScrollDelta.y > 0){
                if(selector.fillAmount < 0.939f){
                    selector.fillAmount = selector.fillAmount + 0.00835f;
                    if(value < 100){
                        scrollSound.Play();
                        value++;
                        selectionValue.text = value.ToString();
                    }
                }
            }

            if(Input.mouseScrollDelta.y < 0){
                if(selector.fillAmount > 0.10943f){
                    selector.fillAmount = selector.fillAmount - 0.00835f;
                    if(value > 0){
                        scrollSound.Play();
                        value--;
                        selectionValue.text = value.ToString();
                    }
                }
            }

            if(Input.GetMouseButtonDown(0)){
                script.checkOutput(selectionValue.text);
            }
        }
    }
}

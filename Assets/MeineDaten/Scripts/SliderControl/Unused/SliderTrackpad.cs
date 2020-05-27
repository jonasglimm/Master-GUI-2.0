using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SliderTrackpad : MonoBehaviour
{
    private Vector3 lastMouseCoordinate = Vector3.zero;
    public Slider slider;
    private bool selected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update(){
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        if(Input.GetMouseButtonDown(0)){
            slider.Select();
            selected = true;
        }  else if(Input.GetMouseButtonUp(0)) {
            selected = false;
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }

        if(mouseDelta.x < 0 ){ // if difference less than zero, moved to left
            // horizontalMovementTxt.text = "Mouse Moved Left";
            lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
            if(selected == true){ // checking if the mouse button is pressed down. 
                slider.value = slider.value -1;
            }
        } else if(mouseDelta.x > 0 ){ // if difference greater than zero, moved to right
            lastMouseCoordinate = Input.mousePosition;
            if(selected == true){
                slider.value = slider.value +1;
                
            }
        } 
       
    }

}

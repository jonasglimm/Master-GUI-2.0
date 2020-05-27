using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnteringTrigger : MonoBehaviour
{
    public MapControlAlt mapControl;

    public void OnTriggerStay(Collider other)
    {
        mapControl.mapCamera.GetComponent<Transform>().position = mapControl.lastPosition;
        Debug.Log("Trigger berührt");
    }

   
}

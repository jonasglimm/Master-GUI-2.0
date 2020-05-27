using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinSouthCollider : MonoBehaviour
{
    public bool pinSouthEntered = false;
    

    private void OnTriggerEnter(Collider other)
    {
        pinSouthEntered = true;
    }

}

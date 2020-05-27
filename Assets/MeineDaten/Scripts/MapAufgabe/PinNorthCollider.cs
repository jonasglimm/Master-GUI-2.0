using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinNorthCollider : MonoBehaviour
{
    public bool pinNorthEntered = false;


    private void OnTriggerEnter(Collider other)
    {
        pinNorthEntered = true;
    }
}

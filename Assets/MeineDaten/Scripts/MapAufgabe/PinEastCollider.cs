using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinEastCollider : MonoBehaviour
{
    public bool pinEastEntered = false;


    private void OnTriggerEnter(Collider other)
    {
        pinEastEntered = true;
    }
}

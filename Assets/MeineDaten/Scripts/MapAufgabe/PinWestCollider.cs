using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinWestCollider : MonoBehaviour
{
    public bool pinWestEntered = false;


    private void OnTriggerEnter(Collider other)
    {
        pinWestEntered = true;
    }
}

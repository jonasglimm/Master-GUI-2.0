using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBlaetterAufgabe : MonoBehaviour
{
    private ValueControlCenter valueControlCenter;
    private GameObject snapOnScroll;
    private AudioSource clickSound;

    public void Awake()
    {
        InitiateVariables();
        EnableBlaetterRectMovement();
        EnableClickSound();
    }

    private void InitiateVariables()
    {
        valueControlCenter = GameObject.Find("BlaetterManager").GetComponent<ValueControlCenter>();
        snapOnScroll = GameObject.Find("SnapOnScroll");
        clickSound = GameObject.Find("BlaetterManager").GetComponent<AudioSource>();
    }

    private void EnableBlaetterRectMovement()
    {
        if (valueControlCenter.touchscreenInput == false)
        {
            snapOnScroll.GetComponent<BlaetterRectMovement>().enabled = true;
        }
        else
        {
            snapOnScroll.GetComponent<BlaetterRectMovement>().enabled = false;
        }
    }

    private void EnableClickSound()
    {
        if (valueControlCenter.touchscreenInput == false)
        {
            clickSound.enabled = true;
        }
        else
        {
            clickSound.enabled = false;
        }
    }
}

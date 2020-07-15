﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBlaetterAufgabe : MonoBehaviour
{
    private ValueControlCenter valueControlCenter;
    private GameObject snapOnScroll;
    private AudioSource clickSound;
    private Button firstButton;

    public void Awake()
    {
        InitiateVariables();
        EnableBlaetterRectMovement();
        ChangeColorForTouchscreen();
        //EnableClickSound();
    }

    private void InitiateVariables()
    {
        valueControlCenter = GameObject.Find("BlaetterManager").GetComponent<ValueControlCenter>();
        snapOnScroll = GameObject.Find("SnapOnScroll");
        clickSound = GameObject.Find("BlaetterManager").GetComponent<AudioSource>();
        firstButton = GameObject.Find("FirstButton").GetComponent<Button>();
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

    private void ChangeColorForTouchscreen()
    {
        if (valueControlCenter.touchscreenInput)
        {
            ColorBlock colorVar = firstButton.colors;
            colorVar.selectedColor = new Color(0.2666667f, 0.4470588f, 0.7686275f, 1);
            colorVar.highlightedColor = new Color(0.2666667f, 0.4470588f, 0.7686275f, 1);
            firstButton.colors = colorVar;
        }
    }
}

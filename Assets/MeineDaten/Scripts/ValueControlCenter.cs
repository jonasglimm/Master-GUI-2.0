using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueControlCenter : MonoBehaviour
{
    [Header("Task overview")]
    public int numberOfTasks; //start value 5 - 

    [Header("Input modality")]
    public bool touchscreenInput;
    public bool touchpadInput; 
    public bool iDriveInput; //not yet used
    public bool gestureInput;

    [Header("Touchpad controls")]
    public float cursorResetTime; //start value 0.3f - 

    [Header("Feedback controls")]
    public float feedbackPanelTime; //start value 0.5f - 

    [Header("Starting objects")]
    public Button startButton;

    [Header("Start requirements")]
    public GameObject modalityWarning;


    private bool[] modalities = new bool[4];

    void Awake()
    {
        CheckModalities();
    }

    public void CheckModalities() // Check if exactly one modality is set to active
    {
        modalities[0] = touchscreenInput;
        modalities[1] = touchpadInput;
        modalities[2] = iDriveInput;
        modalities[3] = gestureInput;

        int counter = 0;
        for (int i = 0; i < modalities.Length; i++)
        {
            if (modalities[i] == true)
            {
                counter++;
            }
        }

        if (counter != 1)
        {
            modalityWarning.SetActive(true);
        }
    }
}

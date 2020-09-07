using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class ControlManager : MonoBehaviour
{
    [Header("Task Values")] // set the tasks
    public bool taskIsTextInput;
    public string[] tasks;
    public int maxValue =100;

    [Header("GUI Elements")] // Set the different GUI-Elements
    public TextMeshProUGUI taskTextField;
    public TextMeshProUGUI errorCountTextField;
    public TextMeshProUGUI currentTaskTextField;
    public TextMeshProUGUI timeTextField;
    public GameObject errorScreen;
    public GameObject successScreen;
    public GameObject completionScreen;
    
    [Header("Task overview")]
    public int totalTasks; // set the number of tasks
    public float activeTime; // set the time for visual feedback panel

    [Header("Input modality")] //checkbox for which modality should currently be used
    public bool touchscreenInput;
    public bool touchpadInput;
    public bool iDriveInput;
    public bool gestureInput;

    [Header("Start requirements")] 
    public GameObject modalityWarning; //warning panel if multiple modalities are activ
    public  float cursorResetTime; //time intervall after which the cursor is hidden to be reset into the screen center

    [Header("Sounds")]
    public AudioSource clickSound;
    
    private int taskNumber; //task counter
    
    private int errors; //error counter
    private bool[] modalities = new bool[4]; 
    private int[] taskList = new int[15]; //list to generate a diverse but still repetitive order of tasks which change depending on the total amount of tasks
    private int currentTaskNumber;
    private int lastTaskElement;
    private DateTime startTime;
    public GameObject startPanel;
    public GameObject startPanelTouchscreen;

    [HideInInspector]
    public bool endscreenIsActive = false;
    [HideInInspector]
    public bool startPanelIsActive;


    // Use this for initialization
    void Start () {
        CheckModalities();
        errors = 0;
        taskNumber = 1;
        taskTextField.text = taskNumber.ToString() + " / "+totalTasks.ToString();
        errorCountTextField.text = errors.ToString();
        CreateTaskOrder();
        SetStartPanel();
        NewTask();
    }

    private void Update() //Check if the supervisor has started or ended the current task
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndScreen();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartTime();
        }

    }

    public void SetStartPanel() //set start panel depending on the modality
    {
        if (touchscreenInput == true)
        {
            startPanel.SetActive(false);
            startPanelTouchscreen.SetActive(true);
            startPanelIsActive = true;
        }
        else
        {
            startPanelTouchscreen.SetActive(false);
            startPanel.SetActive(true);
            startPanelIsActive = true;
        }
    }

    public void StartTime() // Start time for ToT measurement and deactivate all start panels (started via start button or key stroke of "S"
    {
        startTime = System.DateTime.Now;
        clickSound.Play(); //acustic feedback
        startPanel.SetActive(false);
        startPanelTouchscreen.SetActive(false);
        startPanelIsActive = false;
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

    private void NewTask() //generate a new task
    {
        //need the "factor" variable if there should be no task limit
        int factor = taskNumber / taskList.Length; //because the variable "factor" is an "int", the digits after the decimal point are being dropped
        if (taskNumber - (factor * taskList.Length) != 0)
        {
            if (currentTaskNumber != taskList[taskNumber - (factor * taskList.Length) - 1]) // prevent that the same task is selected twice im a row
            {
                currentTaskNumber = taskList[taskNumber - (factor * taskList.Length) - 1];
            }
            else if (currentTaskNumber != taskList[taskNumber - (factor * taskList.Length) - 2]) // prevent that the same task is selected twice im a row
            {
                currentTaskNumber = taskList[taskNumber - (factor * taskList.Length) - 2];
            }
            else if (currentTaskNumber != taskList[taskNumber - (factor * taskList.Length) - 3]) // prevent that the same task is selected twice im a row
            {
                currentTaskNumber = taskList[taskNumber - (factor * taskList.Length) - 3];
            }
        }
        else
        {
            currentTaskNumber = taskList[taskNumber / factor - 1];
        }

        if (taskIsTextInput == false) //To convert the text into a processable variable
        {
            currentTaskTextField.text = currentTaskNumber.ToString();
        }
        else
        {
            // select the next task
            int currentTaskElement = currentTaskNumber * tasks.Length / maxValue;
            currentTaskTextField.text = tasks[currentTaskElement];

            while (currentTaskElement == lastTaskElement)
            {
                if(currentTaskElement == tasks.Length - 1)
                {
                    currentTaskElement--;
                }
                else
                {
                    currentTaskElement++;
                }
            }
            if (touchscreenInput)
            {
                currentTaskTextField.text = tasks[currentTaskElement];
            }
            else
            {
                currentTaskTextField.text = tasks[currentTaskElement].ToUpper(); //Capital letter to fit the keyboard layout
            }
            lastTaskElement = currentTaskElement;
        }
    }


    private void CreateTaskOrder() //Random Order depending on the number of names
{
        //needs to be the same as in SliderTask
        taskList[0] = maxValue * 4 / 7;
        taskList[1] = maxValue * 7 / 9;
        taskList[2] = maxValue / 4 + 1;
        taskList[3] = maxValue * 3 / 5;
        taskList[4] = maxValue * 6 / 7;
        taskList[5] = maxValue * 2 / 9;
        taskList[6] = maxValue * 3 / 4;
        taskList[7] = maxValue * 2 / 7;
        taskList[8] = maxValue / 3;
        taskList[9] = maxValue * 5 / 6;
        taskList[10] = maxValue / 2;
        taskList[11] = maxValue * 1 / 8;
        taskList[12] = maxValue / 4;
        taskList[13] = maxValue * 8 / 10;
        taskList[14] = maxValue * 3 / 10;
}
    
    void updateValues(){
        //show success screen
        StartCoroutine(showSuccessFeedback());
        taskNumber++;
        NewTask();
        taskTextField.text = taskNumber.ToString() + " / "+totalTasks;
    }

     IEnumerator showSuccessFeedback(){
        successScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(activeTime);
        successScreen.SetActive(false);
    }

    IEnumerator showErrorFeedback(){
        errorScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(activeTime);
        errorScreen.SetActive(false);
    }
    // Check if the input was correct
     public void checkOutput(string inputText){
        clickSound.Play();
        string verificationString = currentTaskTextField.text;
        verificationString = verificationString.ToUpper();
        if(verificationString == inputText){
            updateValues();
            if(taskNumber > totalTasks){
                EndScreen();
            }
        } else {
            // show error screen
            StartCoroutine(showErrorFeedback());
            errors++;
            errorCountTextField.text = errors.ToString();
        }
     }

    public void EndScreen() //show end screen and stop the timer for ToT
    {
        var totalTime = System.DateTime.Now - startTime;
        timeTextField.text = totalTime.Minutes.ToString() + " min : " + totalTime.Seconds.ToString() + " sec";
        endscreenIsActive = true;
        completionScreen.SetActive(true);
    }
}

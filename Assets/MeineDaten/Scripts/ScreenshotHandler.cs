using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    public string filename;
    public int sizeMultiplicator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScreenCapture.CaptureScreenshot(filename, sizeMultiplicator);
            Debug.Log("Screenshot gemacht! Dateiname ist " + filename);
        }
    }

}

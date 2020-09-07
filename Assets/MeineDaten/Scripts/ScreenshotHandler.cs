using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    public string filename;
    public int sizeMultiplicator = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ScreenCapture.CaptureScreenshot(filename, sizeMultiplicator);
            Debug.Log("Screenshot gemacht! Dateiname ist " + filename);
        }
    }

}

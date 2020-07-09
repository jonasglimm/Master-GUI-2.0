using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class IDriveController : MonoBehaviour
{
    [HideInInspector]
    public bool movedLeftOnce, movedRightOnce, movedUpOnce, movedDownOnce, pushedOnce;
    [HideInInspector]
    public bool pushStarted, pushHeld, pushEnded;
    [HideInInspector]
    public bool leftStarted, leftHeld, leftEnded;
    [HideInInspector]
    public bool rightStarted, rightHeld, rightEnded;
    [HideInInspector]
    public bool upStarted, upHeld, upEnded;
    [HideInInspector]
    public bool downStarted, downHeld, downEnded;

    [HideInInspector]
    public bool turnedClockwise, turnedCounterclockwise;
    [HideInInspector]
    public int rotationClockwiseSteps, rotationCounterclockwiseSteps;


    [HideInInspector]
    public bool RotaryPush, RotaryLeft, RotaryRight, RotaryUp, RotaryDown, MainOption, MainBack, MainMedia, MainRadio, MainTel, MainNav, MainMenue;
    [HideInInspector]
    public sbyte RotaryEx = 0;

    private bool rotationInLastFrame;

    private SerialPort port = new SerialPort("/dev/tty.usbmodem48692101", 115200, Parity.None, 8, StopBits.One);

    // Start is called before the first frame update
    void Start()
    {
        movedDownOnce = movedLeftOnce = movedRightOnce = movedUpOnce = pushedOnce = false;
        pushStarted = leftStarted = rightStarted = upStarted = downStarted = false;
        pushHeld = leftHeld = upHeld = downHeld = false;
        pushEnded = leftEnded = rightEnded = upEnded = downEnded = true;
        turnedClockwise = turnedCounterclockwise = false;
        rotationClockwiseSteps = rotationCounterclockwiseSteps = 0;

        rotationInLastFrame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!port.IsOpen)
        {
            port.Open();
        }

        byte[] buffer = new byte[10];
        // heading
        //Debug.Log("Start Sync");
		while(buffer[0] != 0x55)
        {
            port.Read(buffer, 0, 1);
            if (buffer[0] != 0x55)
            {
            	//Debug.Log("Wrong Byte");
            }
        }
        //Debug.Log("Start Frame");

        port.Read(buffer, 1, 1);
        if (buffer[1] != 0xaa) return;

        // data
        port.Read(buffer, 2, 3);

        // trailing
        port.Read(buffer, 5, 2);

        // interpret data
        byte MainButtons = buffer[2];
        byte RotaryButtons = buffer[3];
        byte Rotary = buffer[4];
        
        RotaryEx = (sbyte)Rotary;

        RotaryPush = ((RotaryButtons & 0x10) != 0x00);

        RotaryLeft = ((RotaryButtons & 0x02) != 0x00);
		RotaryRight = ((RotaryButtons & 0x04) != 0x00);
		RotaryUp = ((RotaryButtons & 0x01) != 0x00);
		RotaryDown = ((RotaryButtons & 0x08) != 0x00);

		MainOption = ((MainButtons & 0x08) != 0x00); 
		MainBack = ((MainButtons & 0x40) != 0x00); 
		MainMedia = ((MainButtons & 0x20) != 0x00); 
		MainRadio = ((MainButtons & 0x10) != 0x00); 
        MainTel = ((MainButtons & 0x04) != 0x00); 
		MainNav = ((MainButtons & 0x02) != 0x00); 
		MainMenue = ((MainButtons & 0x01) != 0x00);

        //Debug.Log(RotaryPush);
        CheckControllerMovement();
        CheckRotation();
        CheckPushedDown();
        //Debug.Log("RotartyEx = " + RotaryEx);
        //Debug.Log("Clockwise: " + rotationClockwiseSteps);
        //Debug.Log("Counterclockwise: " + rotationCounterclockwiseSteps);

        /*
        if (movedLeftOnce)
        {
            Debug.Log("movedLeftOnce");
        }
        if (movedRightOnce)
        {
            Debug.Log("movedRightOnce");
        }
        if (movedUpOnce)
        {
            Debug.Log("movedUpOnce");
        }
        if (movedDownOnce)
        {
            Debug.Log("movedDownOnce");
        }
        if (pushedOnce)
        {
            Debug.Log("pushedOnce");
        }
        */
        //Debug.Log(RotaryEx);
        //Debug.Log("Done");
    }

    void CheckPushedDown()
    {
        if (RotaryPush)
        {
            if (pushEnded)
            {
                pushStarted = pushedOnce= true;
                pushEnded = false;
            }
            else if (pushStarted)
            {
                pushStarted = pushedOnce = false;
                pushHeld = true;
            }
        }
        else if (!RotaryPush)
        {
            if (pushHeld || pushStarted)
            {
                pushHeld = pushStarted = pushedOnce = false;
                pushEnded = true;
            }
        }
    }

    void CheckControllerMovement()
    {
        CheckLeft();
        CheckRight();
        CheckUp();
        CheckDown();
    }

    void CheckRotation()
    {
        rotationClockwiseSteps = rotationCounterclockwiseSteps = 0;

        if (RotaryEx == 0)
        {
            turnedClockwise = turnedCounterclockwise = false;
        }
        else if (RotaryEx < 0)
        {
            turnedClockwise = true;
            turnedCounterclockwise = false;
            if (Math.Abs(RotaryEx) == (sbyte)4 || Math.Abs(RotaryEx) == (sbyte)8 || Math.Abs(RotaryEx) == (sbyte)16 || Math.Abs(RotaryEx) == (sbyte)20 || Math.Abs(RotaryEx) == (sbyte)24)
            {
                int factor = Math.Abs(RotaryEx) / 4; //int because only the factor is needed
                rotationClockwiseSteps = factor;
            }
            else
            {
                int factor = Math.Abs(RotaryEx) / 4; //int because only the factor is needed
                rotationClockwiseSteps = factor + 1;
            }
        }
        else if (RotaryEx > 0)
        {
            turnedClockwise = false;
            turnedCounterclockwise = true;

            if (Math.Abs(RotaryEx) == (sbyte)4 || Math.Abs(RotaryEx) == (sbyte)8 || Math.Abs(RotaryEx) == (sbyte)16 || Math.Abs(RotaryEx) == (sbyte)20 || Math.Abs(RotaryEx) == (sbyte)24)
            {
                int factor = Math.Abs(RotaryEx) / 4; //int because only the factor is needed
                rotationCounterclockwiseSteps = factor;
            }
            else
            {
                int factor = Math.Abs(RotaryEx) / 4; //int because only the factor is needed
                rotationCounterclockwiseSteps = factor + 1;
            }
        }

        if (rotationClockwiseSteps == 1 || rotationCounterclockwiseSteps == 1) //sometime a rotations takes place during two frames. This if-statment prevents a double step
        {
            if (rotationInLastFrame)
            {
                rotationClockwiseSteps = rotationCounterclockwiseSteps = 0;
                rotationInLastFrame = false;
            }
            else
            {
                rotationInLastFrame = true;
            }
        }
        else
        {
            rotationInLastFrame = false;
        }
    }

    void CheckLeft()
    {
        if (RotaryLeft)
        {
            if (leftEnded)
            {
                leftStarted = movedLeftOnce = true;
                leftEnded = false;
            }
            else if (leftStarted)
            {
                leftStarted = movedLeftOnce = false;
                leftHeld = true;
            }
        }
        else if (!RotaryLeft)
        {
            if (leftHeld || leftStarted)
            {
                leftHeld = leftStarted = movedLeftOnce = false;
                leftEnded = true;
            }
        }

    }
    void CheckRight()
    {
        if (RotaryRight)
        {
            if (rightEnded)
            {
                rightStarted = movedRightOnce = true;
                rightEnded = false;
            }
            else if (rightStarted)
            {
                rightStarted = movedRightOnce = false;
                rightHeld = true;
            }
        }
        else if (!RotaryRight)
        {
            if (rightHeld || rightStarted)
            {
                rightHeld = rightStarted = movedRightOnce = false;
                rightEnded = true;
            }
        }

    }
    void CheckUp()
    {
        if (RotaryUp)
        {
            if (upEnded)
            {
                upStarted = movedUpOnce = true;
                upEnded = false;
            }
            else if (upStarted)
            {
                upStarted = movedUpOnce = false;
                upHeld = true;
            }
        }
        else if (!RotaryUp)
        {
            if (upHeld || upStarted)
            {
                upHeld = upStarted = movedUpOnce = false;
                upEnded = true;
            }
        }

    }
    void CheckDown()
    {
        if (RotaryDown)
        {
            if (downEnded)
            {
                downStarted = movedDownOnce = true;
                downEnded = false;
            }
            else if (downStarted)
            {
                downStarted = movedDownOnce = false;
                downHeld = true;
            }
        }
        else if (!RotaryDown)
        {
            if (downHeld || downStarted)
            {
                downHeld = downStarted = movedDownOnce = false;
                downEnded = true;
            }
        }
    }

    private void Reset()
    {
        movedLeftOnce = movedRightOnce = movedUpOnce = movedDownOnce = false;
        pushedOnce = false;
    }
}

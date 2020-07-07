using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class OldIDriveController : MonoBehaviour
{
    private SerialPort port = new SerialPort("/dev/ttys000", 115200, Parity.None, 8, StopBits.One);

    // Start is called before the first frame update
    void Start()
    {
        port.Open();
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
        while(buffer[0] != 0x55)
        {
            port.Read(buffer, 0, 1);
        }
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
        //BitConverter.;

        //Debug.Log(Rotary);
    }
}

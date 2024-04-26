using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    public static float Throttle { get { return Instance.throttle; } }
    public static float Elevation { get { return Instance.elevation; } }
    public static float RopeLength { get { return Instance.ropeLength; } }

    [Header("Input Action Configuration")]
    [SerializeField]
    private InputActionReference throttleIA;
    [SerializeField]
    private InputActionReference elevationIA;
    [SerializeField]
    private InputActionReference ropeLengthIA;

    [Header("Arduino Configuration")]    
    [SerializeField]
    private string portName = "COM1";
    [SerializeField]
    private int baudRate = 9600;

    [Header("Debug")]
    [SerializeField]
    private float throttle = 0f;
    [SerializeField]
    private float elevation = 0f;
    [SerializeField]
    private float ropeLength = 0f;
    [SerializeField]
    private string recivedData = "";

    private SerialPort port = new();

    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Input Handlers, duplicate: " + name);
            Destroy(this);
            return;
        }

        if (portName == "" || baudRate == 0)
        {
            Debug.LogError("Input Handler PortName or BaudRate not set, object: " + name);
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (port.IsOpen) port.Close();

        port.PortName = portName;
        port.BaudRate = baudRate;
        port.NewLine = "\n";
        port.Encoding = System.Text.Encoding.ASCII;
        port.ReadTimeout = 100;
        port.Open();
    }

    private void OnDisable()
    {
        port.Close();
    }

    void Update()
    {
        //Debug.Log("Port Names: " + string.Join(", ", SerialPort.GetPortNames()));

        if (port.IsOpen)
        {
            // Arduino Input
            // data format is throttle, rope length, elevation up, and elevation down values as a line terminated string with items separated by underscores.

            recivedData += port.ReadExisting();
            string[] lines = recivedData.Split('\n');
            if (lines.Length < 2) return; // return if no line feed character is found.

            string line = lines[lines.Length - 2]; // get the most recent "complete" line.
            string[] data = line.Split('_'); // separate items
            if (data.Length != 2) return; // if not all items are present line is not complete.

            recivedData = lines[lines.Length - 1]; // assign the most recent incomplete line to recived data.
            int throttleReading = int.Parse(data[0]);
            int ropeReading = int.Parse(data[1]);
            int elevationUpReading = int.Parse(data[2]);
            int elevationDownReading = int.Parse(data[3]);

            // Print raw input values
            Debug.Log($"Throttle: { throttleReading }\nRope: { ropeReading }\nElevation Up: { elevationUpReading }\nElevation Down: { elevationDownReading }");

            throttle = Math.Clamp(throttleReading, 0, 1);
            ropeLength = ropeReading / 1000;
            elevation = elevationUpReading - elevationDownReading;
        }
        else
        {
            // Keyboard Input
            throttle = 0.5f + throttleIA.action.ReadValue<float>() / 2f;
            elevation = elevationIA.action.ReadValue<float>();
            ropeLength = ropeLengthIA.action.ReadValue<float>();
        }
    }
}

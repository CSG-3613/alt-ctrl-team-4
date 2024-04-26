using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    public float Throttle { get { return throttle; } }
    public float Elevation { get { return elevation; } }
    public float RopeLength { get { return ropeLength; } }

    [Header("Input Action Configuration")]
    [SerializeField]
    private InputActionReference throttleIA;
    [SerializeField]
    private InputActionReference elevationIA;
    [SerializeField]
    private InputActionReference ropeLengthIA;

    private SerialPort port = new();

    [Header("Keyboard Configuration")]
    public float ScrollFactor = 0.1f;

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

    public string recivedData = "";

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
        port.PortName = portName;
        port.BaudRate = baudRate;
        port.NewLine = "\n";
        port.Encoding = System.Text.Encoding.ASCII;
        port.ReadTimeout = 100;
        port.Open();
        
        //port.DataReceived += Port_DataReceived;
    }

    private void OnDisable()
    {
        port.Close();

        port.DataReceived -= Port_DataReceived;
    }

    // Arduino Input
    private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        //try
        //{
        //    string str = port.ReadLine();
        //} 
        //catch
        //{
        //
        //}
    }

    void Update()
    {
        //Debug.Log("Port Names: " + string.Join(", ", SerialPort.GetPortNames()));

        // Keyboard Input
        //throttle = 0.5f + throttleIA.action.ReadValue<float>() / 2f;
        //
        //elevation = elevationIA.action.ReadValue<float>();
        //
        //ropeLength = ropeLengthIA.action.ReadValue<float>();

        if (port.IsOpen)
        {
            recivedData += port.ReadExisting();
            string[] lines = recivedData.Split('\n');
            if (lines.Length < 2) return;

            try
            {
                //string line = port.ReadLine();
                string line = lines[lines.Length - 2];
                string[] data = line.Split('_');
                if (data.Length != 2) return;

                recivedData = lines[lines.Length - 1];
                int throttleReading = int.Parse(data[0]);
                int ropeReading = int.Parse(data[1]);

                Debug.Log($"Throttle: { throttleReading }\nRope: { ropeReading }");

                throttle = Math.Clamp(throttleReading, 0, 1);
                ropeLength = ropeReading / 1000;

            }
            catch { }
        }
    }
}

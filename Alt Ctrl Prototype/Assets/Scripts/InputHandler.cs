using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using System.Linq;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }

    public static float Throttle { get { return Instance.throttle; } }
    public static float Elevation { get { return Instance.elevation; } }
    public static float RopeLength { get { return Instance.ropeLength; } }

    private static string _COMPort = null;

    [Tooltip("The maximum length of the rope")]
    public float MaxRopeLength = 25f;

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
        if (_COMPort != null) Instance.portName = _COMPort;
    }

    public static List<string> GetCOMPortNames()
    {
        return SerialPort.GetPortNames().ToList();
    }

    public static bool SetCOMPort(string portName)
    {
        if (!GetCOMPortNames().Contains(portName)) return false;

        if (Instance == null) _COMPort = portName;
        else if (Instance.port.IsOpen)
        {
            Instance.OnDisable();
            Instance.portName = portName;
            Instance.OnEnable();
            return Instance.port.IsOpen;
        }
        else
        {
            Instance.portName = portName;
        }

        return true;
    }

    private void OnEnable()
    {
        if (port.IsOpen) port.Close();

        port.PortName = portName;
        port.BaudRate = baudRate;
        port.NewLine = "\n";
        port.Encoding = System.Text.Encoding.ASCII;
        port.ReadTimeout = 100;
        try
        {
            port.Open();
        }
        catch { }
    }

    private void OnDisable()
    {
        if (port.IsOpen) port.Close();
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
            if (data.Length != 4) return; // if not all items are present line is not complete.

            recivedData = lines[lines.Length - 1]; // assign the most recent incomplete line to recived data.
            int throttleReading = int.Parse(data[0]);       // domain: 0..100
            int ropeReading = Mathf.Abs(int.Parse(data[1]));           // domain: 0..~7000
            int elevationUpReading = int.Parse(data[2]);    // domain: 0..1
            int elevationDownReading = int.Parse(data[3]);  // domain: 0..1

            // Print raw input values
            Debug.Log($"Throttle: { throttleReading }\nRope: { ropeReading }\nElevation Up: { elevationUpReading }\nElevation Down: { elevationDownReading }");

            throttle = Math.Clamp(throttleReading, 0, 100) / 100f;                       // range: 0..100
            ropeLength = Mathf.Clamp(ropeReading / 100f, 0f, MaxRopeLength);    // range: 0..MaxRopeLength
            elevation = elevationUpReading - elevationDownReading;              // range: [-1, 1]
        }
        else
        {
            // Keyboard Input
            throttle = 0.5f + throttleIA.action.ReadValue<float>() / 2f;
            elevation = elevationIA.action.ReadValue<float>();
            ropeLength = Mathf.Clamp(ropeLength + Time.deltaTime * ropeLengthIA.action.ReadValue<float>() * 5f, 0, MaxRopeLength);
        }
    }
}

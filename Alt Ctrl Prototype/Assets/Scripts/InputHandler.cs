using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.InputSystem;

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

    private SerialPort stream;

    [Header("Keyboard Configuration")]
    public float ScrollFactor = 0.1f;

    [Header("Arduino Configuration")]    
    [SerializeField]
    private string portName = "";

    [SerializeField]
    private int baudRate = 0;

    [Header("Debug")]
    [SerializeField]
    private float throttle = 0f;
    [SerializeField]
    private float elevation = 0f;
    [SerializeField]
    private float ropeLength = 0f;

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
        
        //stream = new SerialPort(portName, baudRate);
        //stream.Open();
    }

    void Update()
    {
        // Keyboard Input
        throttle = 0.5f + throttleIA.action.ReadValue<float>() / 2f;
        
        elevation = elevationIA.action.ReadValue<float>();
        
        ropeLength = ropeLengthIA.action.ReadValue<float>();

        // Arduino Input
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    [SerializeField]
    private float throttle = 0f;
    [SerializeField]
    private float elevation = 0f;
    [SerializeField]
    private float ropeLength = 0f;

    public float Throttle { get { return throttle; } }
    public float Elevation { get { return elevation; } }
    public float RopeLength { get { return ropeLength; } }

    private PlayerControls inputs;

    private SerialPort stream;

    [Header("Keyboard Configuration")]
    public float ScrollFactor = 0.1f;

    [Header("Arduino Configuration")]    
    [SerializeField]
    private string portName = "";

    [SerializeField]
    private int baudRate = 0;

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
        
        inputs = new PlayerControls();
        inputs.Enable();

        //stream = new SerialPort(portName, baudRate);
        //stream.Open();
    }

    void Update()
    {
        // Keyboard Input
        throttle = 0.5f + inputs.Standard.Throttle.ReadValue<float>() / 2f;
        
        elevation = inputs.Standard.Elevation.ReadValue<float>();
        
        ropeLength = inputs.Standard.RopeLength.ReadValue<float>();

        // Arduino Input
        
    }
}

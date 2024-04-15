using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Input Configuration")]
    public float ScrollFactor = 0.1f;

    [Header("Behavior Configuration")]
    public Vector2 MaxVelocity = Vector2.one;

    [Header("Required Configuration")]
    public Transform RotorBase;
    public Transform Rotor;
    public Transform TailRotor;

    [Tooltip("The angle of the helicopter when at full throttle")]
    public float MaxTiltAngle = 15f;

    [SerializeField]
    private float _throttle = 0f;
    public float Throttle { get { return _throttle; } }

    public float Elevation { get; private set; } = 0f;

    public float RopeLength { get; private set; } = 0f;

    private PlayerControls inputActions;
    
    private Rigidbody _rigidbody;

    private const string _portName = "";

    private SerialPort _stream;

    void Start()
    {
        Instance = this;

        inputActions = new PlayerControls();
        inputActions.Standard.Enable();

        _rigidbody = GetComponent<Rigidbody>();

        // Setup Arduino Inputs
        
        //stream = new SerialPort(PortName);
        //stream.Open();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    float throttleDelta = Input.mouseScrollDelta.y * ScrollFactor;
    //    Throttle = Mathf.Clamp01(Throttle + throttleDelta);

    //    float yInput = (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f);

    //    Vector3 position = transform.position;
    //    position.x += Throttle * MaxVelocity.x * Time.deltaTime;
    //    position.y += yInput * MaxVelocity.y * Time.deltaTime;
    //    transform.position = position;

    //    transform.eulerAngles = new Vector3(MaxTiltAngle * Throttle, 90f, 0f);
    //}

    private void Update()
    {
        RotorBase.localEulerAngles = new Vector3(Throttle * MaxTiltAngle, 0f);

        var rotation = Rotor.localEulerAngles;
        rotation.y += Throttle;
        Rotor.localEulerAngles = rotation;
    }

    private void FixedUpdate()
    {
        float throttleDelta = inputActions.Standard.Throttle.ReadValue<float>() * ScrollFactor;
        //Debug.Log("Throttle Delta: " + throttleDelta + "\t\tScroll Delta: " + Input.mouseScrollDelta.y);
        _throttle = Mathf.Clamp01(_throttle + throttleDelta);

        float yInput = (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f);

        _rigidbody.AddForce(0, 9.81f, 0, ForceMode.Acceleration);
    }
}

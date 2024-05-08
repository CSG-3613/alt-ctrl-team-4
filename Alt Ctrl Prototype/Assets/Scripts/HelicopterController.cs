using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    public static HelicopterController Instance { get; private set; }

    [Header("Behavior Configuration")]
    [Tooltip("The maximum speed of the helicopter")]
    public float MaxSpeed = 1f;
    [Tooltip("The angle of the helicopter when at full throttle")]
    public float MaxAngle = 15f;
    [Tooltip("The rotational velocity of the rotor at full throttle")]
    public float RotorSpeed = 1f;
    
    public float throttleLerpFactor = 0.01f;

    [Header("Animation Configuration")]
    [SerializeField]
    private Transform rotorBase;
    [SerializeField]
    private Transform rotor;
    [SerializeField]
    private Transform tailRotor;

    private float realThrottle = 0f;
    private float rotorAngle = 0f;
    private float tailAngle = 0f;

    private Rigidbody rb;

    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple HelicopterConroller Instances, duplicate: " + name);
            Destroy(this);
            return;
        }

        Instance = this;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        realThrottle = Mathf.Lerp(realThrottle, InputHandler.Throttle, throttleLerpFactor);

        float baseAngle = realThrottle * MaxAngle / 2f;
        transform.localEulerAngles = new Vector3(baseAngle, 90f, 0);
        rotorBase.localEulerAngles = new Vector3(baseAngle, 0, 0);

        float rotorAngleDelta = (0.5f + realThrottle / 2f) * RotorSpeed * Time.deltaTime;
        rotorAngle = (rotorAngle + rotorAngleDelta) % 360f;
        tailAngle = (tailAngle + rotorAngleDelta * 0.7f) % 360f;
        
        rotor.localEulerAngles = new Vector3(0, rotorAngle);
        tailRotor.localEulerAngles = new Vector3(tailAngle, 0);
        
        rb.velocity = new Vector3(realThrottle * MaxSpeed * Mathf.Cos(Mathf.Deg2Rad * baseAngle), InputHandler.Elevation * MaxSpeed / 10f * Mathf.Cos(Mathf.Deg2Rad * baseAngle));
    }
}

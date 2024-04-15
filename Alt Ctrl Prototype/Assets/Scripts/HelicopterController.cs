using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    public static HelicopterController Instance;

    [Header("Behavior Configuration")]
    [Tooltip("The maximum speed of the helicopter")]
    public float MaxSpeed = 1f;
    [Tooltip("The angle of the helicopter when at full throttle")]
    public float MaxAngle = 15f;
    [Tooltip("The rotational velocity of the rotor at full throttle")]
    public float RotorSpeed = 1f;

    [Header("Animation Configuration")]
    [SerializeField]
    private Transform rotorBase;
    [SerializeField]
    private Transform rotor;
    [SerializeField]
    private Transform tailRotor;

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
        float baseAngle = InputHandler.Instance.Throttle * MaxAngle / 2f;
        transform.localEulerAngles = new Vector3(baseAngle, 90f, 0);
        rotorBase.localEulerAngles = new Vector3(baseAngle, 0, 0);

        rotor.localEulerAngles += new Vector3(0, (0.5f + InputHandler.Instance.Throttle / 2f) * RotorSpeed * Time.deltaTime);
        tailRotor.localEulerAngles += new Vector3((0.5f + InputHandler.Instance.Throttle / 2f) * RotorSpeed * Time.deltaTime, 0);
        //tailRotor.localEulerAngles += new Vector3(0.5f * RotorSpeed * Time.deltaTime, 0f, 0f);

        rb.velocity = new Vector3(InputHandler.Instance.Throttle * MaxSpeed * Mathf.Sin(Mathf.Deg2Rad * baseAngle), InputHandler.Instance.Elevation * MaxSpeed * Mathf.Cos(Mathf.Deg2Rad * baseAngle));
    }
}

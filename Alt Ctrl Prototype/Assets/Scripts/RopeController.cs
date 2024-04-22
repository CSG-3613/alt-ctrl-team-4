using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public static RopeController Instance { get; private set; }

    [Tooltip("The maximum length of the rope")]
    public float MaxRopeLength = 25f;

    [SerializeField]
    private GameObject ropePrefab;

    [SerializeField]
    private Transform ropeRoot;

    [SerializeField]
    private float ropeLength = 0;

    public List<GameObject> segments = new();

    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad1)) ropeLength = Mathf.Min(MaxRopeLength, ropeLength + Time.deltaTime);
        if (Input.GetKey(KeyCode.Keypad2)) ropeLength = Mathf.Max(0, ropeLength - Time.deltaTime);

        while (segments.Count < Mathf.CeilToInt(ropeLength)) addSegment();

        while (segments.Count > Mathf.CeilToInt(ropeLength)) removeSegment();

        if (segments.Count > 0)
        {
            var Child = segments.Last().transform.GetChild(0);
            var remainder = ropeLength % 1f;
            Child.localScale = new Vector3(1, remainder, 1);
            Child.localPosition = new Vector3(1, 1 - remainder, 1);
        }
    }

    [ContextMenu("Add Segment")]
    void addSegment()
    {
        GameObject newSegment = Instantiate(ropePrefab, ropeRoot);

        if (segments.Count > 0)
        {
            GameObject lastSegment = segments.Last();
            newSegment.transform.position = lastSegment.transform.position - lastSegment.transform.up / 2f;
        }

        Joint joint = newSegment.GetComponent<Joint>();
        if (segments.Count == 0)
        {
            joint.autoConfigureConnectedAnchor = true;
            joint.connectedBody = ropeRoot.GetComponent<Rigidbody>();
        }
        else joint.connectedBody = segments.Last().GetComponent<Rigidbody>();

        segments.Add(newSegment);
    }

    [ContextMenu("Remove Segment")]
    void removeSegment()
    {
        if (segments.Count > 0)
        {
            GameObject segment = segments.Last();
            segments.RemoveAt(segments.Count - 1);
            Destroy(segment);
        }
    }
}

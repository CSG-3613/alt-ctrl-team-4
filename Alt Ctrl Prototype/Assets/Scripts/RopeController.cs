using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopeController : MonoBehaviour
{


    [SerializeField]
    private GameObject ropePrefab;

    [SerializeField]
    private Transform ropeRoot;

    [SerializeField]
    private List<GameObject> segments = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)) addSegment();
        if (Input.GetKeyDown(KeyCode.Keypad2)) removeSegment();
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

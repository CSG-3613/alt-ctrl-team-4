using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public static RopeController Instance { get; private set; }

    [SerializeField]
    private GameObject ropePrefab;

    [SerializeField]
    public Transform ropeRoot;

    [SerializeField]
    private float ropeLength = 0;

    public List<GameObject> segments = new();

    public List<FixedJoint> connected = new();

    public bool overwriteInputLength = false;

    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (!overwriteInputLength) ropeLength = InputHandler.RopeLength;

        while (segments.Count < Mathf.CeilToInt(ropeLength)) addSegment();

        while (segments.Count > Mathf.CeilToInt(ropeLength)) removeSegment();

        if (segments.Count > 0)
        {
            var child = segments.First().transform.GetChild(0);
            var remainder = ropeLength % 1f;
            child.localScale = new Vector3(1, remainder, 1);
            child.localPosition = new Vector3(0, -remainder, 0);
        }
    }

    [ContextMenu("Add Segment")]
    void addSegment()
    {
        GameObject newSegment = Instantiate(ropePrefab, ropeRoot);

        newSegment.transform.position = ropeRoot.position;
        
        if (segments.Count > 0)
        {
            GameObject lastSegment = segments.Last();
            //lastSegment.transform.GetChild(0).localScale = Vector3.one;
            lastSegment.transform.position = newSegment.transform.position - newSegment.transform.up / 2f;
            for (int i = segments.Count - 1; i > 0; i--)
            {
                segments[i - 1].transform.position = segments[i].transform.position - segments[i].transform.up / 2f;
            }
            
            HingeJoint newJoint = newSegment.GetComponent<HingeJoint>();
            HingeJoint oldJoint = lastSegment.GetComponent<HingeJoint>();

            newJoint.connectedAnchor = new Vector3(0, 0);
            newJoint.connectedBody = ropeRoot.GetComponent<Rigidbody>();
            oldJoint.connectedAnchor = new Vector3(0, -2f);
            oldJoint.connectedBody = newJoint.GetComponent<Rigidbody>();
        }

        segments.Add(newSegment);
    }

    [ContextMenu("Remove Segment")]
    void removeSegment()
    {
        if (segments.Count > 0)
        {
            GameObject segment = segments.Last();
            if (segments.Count > 1)
            {
                GameObject previous = segments[segments.Count - 2];
                
                Rigidbody root = ropeRoot.GetComponent<Rigidbody>();
                HingeJoint joint = previous.GetComponent<HingeJoint>();
                joint.connectedBody = root;
                joint.connectedAnchor = new Vector3(0, 0, 0);
                previous.transform.position = ropeRoot.transform.position;

                for (int i = segments.Count - 2; i > 0; i--)
                {
                    segments[i - 1].transform.position = segments[i].transform.position - segments[i].transform.up / 2f;
                }
            }

            segments.RemoveAt(segments.Count - 1);
            Destroy(segment);
        }
    }
}

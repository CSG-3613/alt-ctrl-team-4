using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public static RopeController Instance { get; private set; }

    public Transform ropeRoot;

    [SerializeField]
    private GameObject ropePrefab;

    [SerializeField]
    private float ropeLength = 0;

    public List<GameObject> segments = new();

    public List<AttachableObject> attached = new();

    [Header("Debug")]
    [SerializeField]
    private bool overwriteInputLength = false;

    private void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple RopeConroller Instances, duplicate: " + name);
            Destroy(this);
            return;
        }

        Instance = this;
    }

    void FixedUpdate()
    {
        if (!overwriteInputLength) ropeLength = InputHandler.RopeLength;

        while (segments.Count < Mathf.CeilToInt(ropeLength)) AddSegment();

        while (segments.Count > Mathf.CeilToInt(ropeLength)) RemoveSegment();

        if (segments.Count > 0)
        {
            var child = segments.First().transform.GetChild(0);
            var remainder = ropeLength % 1f;
            child.localScale = new Vector3(1, remainder, 1);
            child.localPosition = new Vector3(0, -remainder, 0);
        }

        attached.ForEach(x =>
        {
            var ropeSegment = x.joint.connectedBody;

            x.joint.connectedBody = null;
            x.transform.position = ropeSegment.transform.position - ropeSegment.transform.up * ((InputHandler.RopeLength % 1f) / 2f);
            x.joint.connectedBody = ropeSegment;
        });
    }

    [ContextMenu("Add Segment")]
    void AddSegment()
    {
        GameObject newSegment = Instantiate(ropePrefab, ropeRoot);

        newSegment.transform.position = ropeRoot.position;

        segments.Add(newSegment);
        if (segments.Count > 1)
        {
            GameObject previousSegment = segments[^2];

            for (int i = segments.Count - 1; i > 0; i--)
            {
                segments[i].transform.SetPositionAndRotation(segments[i - 1].transform.position, segments[i - 1].transform.rotation);
            }

            segments[0].transform.SetPositionAndRotation(segments[1].transform.position - segments[1].transform.up / 2f, segments[1].transform.rotation);

            HingeJoint newJoint = newSegment.GetComponent<HingeJoint>();
            HingeJoint oldJoint = previousSegment.GetComponent<HingeJoint>();

            newJoint.connectedAnchor = new Vector3(0, 0);
            newJoint.connectedBody = ropeRoot.GetComponent<Rigidbody>();
            oldJoint.connectedAnchor = new Vector3(0, -2f);
            oldJoint.connectedBody = newJoint.GetComponent<Rigidbody>();
        }
        else
        {
            newSegment.GetComponent<HingeJoint>().connectedBody = ropeRoot.GetComponent<Rigidbody>();
        }
    }

    [ContextMenu("Remove Segment")]
    void RemoveSegment()
    {
        if (segments.Count > 0)
        {
            GameObject segment = segments[^1];
            if (segments.Count > 1)
            {
                for (int i = 0; i < segments.Count - 1; i++)
                {
                    segments[i].transform.SetPositionAndRotation(segments[i + 1].transform.position, segments[i + 1].transform.rotation);
                }

                HingeJoint joint = segments[^2].GetComponent<HingeJoint>();
                joint.connectedBody = ropeRoot.GetComponent<Rigidbody>();
                joint.connectedAnchor = new Vector3(0, 0, 0);
            }

            Destroy(segment);
            segments.RemoveAt(segments.Count - 1);
        }
    }
}

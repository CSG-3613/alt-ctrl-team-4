using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableObject : MonoBehaviour
{
    [Tooltip("The distance form the rope root that the object will be collected")]
    public float CollectionDistance = 1f;

    [Tooltip("The distance form a rope segment at which the object will be become attached")]
    public float AttachDistance = 1f;

    private FixedJoint joint;

    private void Update()
    {
        if (joint != null)
        {
            if (joint.connectedBody == null)
            {
                Destroy(joint);
            }

            if (Vector3.Distance(transform.position, RopeController.Instance.ropeRoot.position) < CollectionDistance)
            {
                Destroy(gameObject);

                // TODO: game score
            }

            var ropeSegment = joint.connectedBody;

            joint.connectedBody = null;
            transform.position = ropeSegment.transform.position - ropeSegment.transform.up * ((InputHandler.RopeLength % 1f) / 2f);
            joint.connectedBody = ropeSegment;
        }

        if (joint == null)
        {
            if (Vector3.Distance(transform.position, RopeController.Instance.ropeRoot.position) > 10f) return;

            GameObject closest = null;
            float closestDistance = AttachDistance;
            foreach (GameObject segment in RopeController.Instance.segments)
            {
                float distance = Vector3.Distance(segment.transform.position, transform.position);
                if (distance < closestDistance) closest = segment;
            }

            if (closest != null)
            {
                joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = closest.GetComponent<Rigidbody>();
            }
        }
    }

}

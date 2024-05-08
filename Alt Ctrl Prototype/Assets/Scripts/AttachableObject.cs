using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableObject : MonoBehaviour
{
    [Tooltip("The distance form the rope root that the object will be collected")]
    public float CollectionDistance = 1f;

    [Tooltip("The distance form a rope segment at which the object will be become attached")]
    public float AttachDistance = 1f;

    [Tooltip("The number of points awarded when retrived by the helicopter")]
    public int PointValue = 10;

    public FixedJoint joint;

    private void FixedUpdate()
    {
        if (joint != null)
        {
            if (joint.connectedBody == null)
            {
                RopeController.Instance.attached.Remove(this);
                Destroy(joint);
            }

            if (Vector3.Distance(transform.position, RopeController.Instance.ropeRoot.position) < CollectionDistance)
            {
                RopeController.Instance.attached.Remove(this);
                Destroy(gameObject);

                GameManager.Score += PointValue;
            }
        }

        if (joint == null)
        {
            if (Mathf.Abs(transform.position.x - RopeController.Instance.ropeRoot.position.x) > 30f) return;

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
                RopeController.Instance.attached.Add(this);
            }
        }
    }

}

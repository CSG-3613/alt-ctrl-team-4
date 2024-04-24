using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttachableObject : MonoBehaviour
{
    [Tooltip("The maximum difference in relative velocity untill the object is dropped")]
    public float MaxVelocityDifference = 10f;

    private FixedJoint joint;

    private void Start()
    {
        joint = GetComponent<FixedJoint>();
    }

    private void Update()
    {
        if (joint.connectedBody == null)
        {
            GameObject closest = null;
            float closestDistance = 1;
            foreach (GameObject segment in RopeController.Instance.segments)
            {
                float distance = Vector3.Distance(segment.transform.position, transform.position);
                if (distance < closestDistance) closest = segment;
            }

            if (closest != null)
            {
                joint.connectedBody = closest.GetComponent<Rigidbody>();
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (attach == null)
    //    {
    //        var relativeVelocity = other.attachedRigidbody.velocity - rb.velocity;
    //        if (relativeVelocity.magnitude < MaxVelocityDifference)
    //        {
    //            attach = other.attachedRigidbody;
    //            offset = other.transform.position - transform.position;
    //        }
    //    }

    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    var relativeVelocity = other.attachedRigidbody.velocity - rb.velocity;
    //    if (relativeVelocity.magnitude > MaxVelocityDifference)
    //    {
    //        attach = null;
    //    }
    //}
}

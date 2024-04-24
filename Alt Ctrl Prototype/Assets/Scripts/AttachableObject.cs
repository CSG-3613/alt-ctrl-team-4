using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableObject : MonoBehaviour
{
    [Tooltip("The maximum difference in relative velocity untill the object is dropped")]
    public float MaxVelocityDifference = 10f;

    private FixedJoint joint;

    private void Update()
    {
        if (joint != null && joint.connectedBody == null)
        {
            RopeController.Instance.connected.Remove(joint);
            Destroy(joint);
        }

        if (joint == null)
        {
            GameObject closest = null;
            float closestDistance = 1;
            foreach (GameObject segment in RopeController.Instance.segments)
            {
                float distance = Vector3.Distance(segment.transform.position, transform.position);
                if (distance < closestDistance) closest = segment;
            }

            Debug.Log($"closest: {closestDistance}, { ((closest == null) ? "null" : closest.name) }");

            if (closest != null)
            {
                joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = closest.GetComponent<Rigidbody>();
                RopeController.Instance.connected.Add(joint);
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

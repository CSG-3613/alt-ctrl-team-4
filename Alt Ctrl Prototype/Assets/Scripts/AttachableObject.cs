using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttachableObject : MonoBehaviour
{
    [Tooltip("The maximum difference in relative velocity untill the object is dropped")]
    public float MaxVelocityDifference = 10f;

    private Rigidbody rb;

    private Rigidbody attach;
    private Vector3 offset;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (attach != null) transform.position = attach.position + offset;

        GameObject closest = null;
        foreach (GameObject segment in RopeController.Instance.segments)
        {
            if (Vector3.Distance(segment.transform.position, transform.position) < 1) closest = segment;
        }

        if (closest != null)
        {
            attach = closest.GetComponent<Rigidbody>();
            offset = transform.position - closest.transform.position;
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

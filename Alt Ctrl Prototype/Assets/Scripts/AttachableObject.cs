using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableObject : MonoBehaviour
{
    [Tooltip("The distance form the rope root that the object will be collected")]
    public float CollectionDistance = 1f;

    [SerializeField]
    private FixedJoint joint;

    private void Update()
    {
        if (joint != null)
        {
            if (joint.connectedBody == null)
            {
                RopeController.Instance.connected.Remove(joint);
                Destroy(joint);
            }

            if (Vector3.Distance(transform.position, RopeController.Instance.ropeRoot.position) < CollectionDistance)
            {
                Destroy(gameObject);

                // TODO: game score
            }

            var temp = joint.connectedBody;

            joint.connectedBody = null;
            transform.position = temp.transform.position - temp.transform.up * ((InputHandler.RopeLength % 1f) / 2f);
            joint.connectedBody = temp;
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

}

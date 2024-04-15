/**
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public bool HasFixedHeight = false;
    
    public float FixedHeight = 0;
    
    [Range(0f, 1f)]
    public float LerpFactor = 0.5f;

    public Vector3 TargetOffset = Vector3.zero;

    public Transform Target;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position + TargetOffset, 0.5f);
    }
}

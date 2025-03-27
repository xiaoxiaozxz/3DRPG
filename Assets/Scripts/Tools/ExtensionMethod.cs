using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    private const float dotThreshold = 0.5f;
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var VectorToTarget = target.position - transform.position;
        VectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, VectorToTarget);

        return dot >= dotThreshold;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobePosition : MonoBehaviour
{
    public Transform center;
    public float radius = 1;
    public float radiusOfNew = .2f;
    [Range(-90, 90)] public float latitude;
    [Range(-180, 180)] public float longitude;

    private Vector3 newDirection;

    public void OnDrawGizmos()
    {
        if (center != null)
        {
            newDirection = Quaternion.Euler(0f, longitude * -1, latitude) * center.right;
            Gizmos.DrawSphere(center.position + (newDirection * radius), radiusOfNew);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 3f;
    [SerializeField] private Vector3 offset;

    void Update()
    {
        var elapsedTime = Time.deltaTime;
        var desiredPosition = target.position + offset;
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, elapsedTime * smoothSpeed);
        
        transform.position = smoothedPosition;

    }
}

using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothingFactor = 0.3f;

    private float _elapsedTime;
    
    private void Start()
    {
        transform.position = target.position + offset;
    }

    private void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothingFactor * _elapsedTime);

    }
}

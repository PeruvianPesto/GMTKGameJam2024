using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] public Transform target;

    public float leftEdgeX;
    public float rightEdgeX; 

    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        //Debug.Log("Target position x: " + target.position.x); 
        if (target.position.x > leftEdgeX && target.position.x < rightEdgeX){
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    
    }
}

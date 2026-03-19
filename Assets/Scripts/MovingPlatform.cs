using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 3f;
    public float speed = 2f;
    public bool vertical = false; 

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 direction = vertical ? Vector3.up : Vector3.right;
        transform.position = startPos + direction * Mathf.Sin(Time.time * speed) * moveDistance;
    }
}

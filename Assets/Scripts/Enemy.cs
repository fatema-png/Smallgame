using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public float speed = 2f;
    private bool movingToPoint2 = true;

    void Update()
    {
        if (point1 == null || point2 == null) return;

        if (movingToPoint2)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, point2.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, point2.position) < 0.1f)
                movingToPoint2 = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(
                transform.position, point1.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, point1.position) < 0.1f)
                movingToPoint2 = true;
        }
    }
}

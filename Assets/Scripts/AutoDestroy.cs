using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private Transform player;
    public float destroyDistance = 40f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        if (transform.position.x < player.position.x - destroyDistance)
            Destroy(gameObject);
    }
}
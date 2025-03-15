using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;

    private Rigidbody2D rb;
    private Vector2 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        rb.WakeUp();
    }

    private void OnDisable()
    {
        rb.velocity = Vector2.zero;
        rb.Sleep();
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y = rb.velocity.y; // Maintain existing vertical velocity

        // Apply gravity manually if needed
        if (!Physics2D.Raycast(transform.position, Vector2.down, 0.1f))
        {
            velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;
        }

        rb.velocity = velocity; // Use velocity instead of MovePosition

        // Wall collision detection and direction change
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, LayerMask.GetMask("Pipe"));
        if (hit.collider != null)
        {
            direction = -direction;
        }

        // Flip sprite direction
        transform.localScale = new Vector3(direction.x > 0 ? -1 : 1, 1, 1);
    }
}

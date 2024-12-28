using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public Vector2 initialVelocity;
    public float damage;
    public float lifetime = 5f; // Bullet lifetime before self-destruction

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb != null)
        {
            _rb.linearVelocity = initialVelocity; // Set the initial velocity
        }

        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    // Detect collisions with the player or other objects
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatus playerStatus = collision.gameObject.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damage); // Apply damage to the player
            }
        }

        // Destroy the bullet on collision
        Destroy(gameObject);
    }
}
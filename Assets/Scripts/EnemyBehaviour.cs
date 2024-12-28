using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectRange = 5f;
    public float hoverHeight = 1.5f;
    public float hoverSpeed = 2f;
    public float targetDistance = 2f;

    public GameObject bulletPrefab; // Bullet prefab to instantiate
    public float bulletSpeed = 5f;  // Speed of the fired bullet
    public float reloadTime = 2f;   // Time between shots

    private Rigidbody2D _rb;
    private Transform _playerTransform;
    public LayerMask wallLayerMask;
    public LayerMask groundLayerMask; // For ground detection

    private float _reloadTimer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _reloadTimer = reloadTime; // Initialize reload timer
    }

    private void Update()
    {
        Hover();
        DetectAndFollowPlayer();
        HandleShooting();
    }

    private void Hover()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, hoverHeight + 0.5f, groundLayerMask);

        if (hit.collider != null)
        {
            float heightDifference = hoverHeight - hit.distance;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Lerp(_rb.linearVelocity.y, heightDifference * hoverSpeed, Time.deltaTime));
        }
    }

    private void DetectAndFollowPlayer()
    {
        if (_playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer <= detectRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _playerTransform.position - transform.position, detectRange, wallLayerMask);

            if (hit.collider == null || hit.collider.CompareTag("Player"))
            {
                Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;

                if (distanceToPlayer > targetDistance)
                {
                    _rb.linearVelocity = new Vector2(directionToPlayer.x * moveSpeed, _rb.linearVelocity.y);
                }
                else
                {
                    _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
                }
            }
        }
        else
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }
    }

    private void HandleShooting()
    {
        if (_playerTransform == null) return;

        _reloadTimer -= Time.deltaTime;
        if (_reloadTimer <= 0 && Vector2.Distance(transform.position, _playerTransform.position) <= detectRange)
        {
            FireBullet();
            _reloadTimer = reloadTime;
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab == null || _playerTransform == null) return;

        // Instantiate bullet at enemy position
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Calculate direction towards the player
        Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;

        // Assign velocity to the bullet's Rigidbody2D
        BulletBehaviour bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
        if (bulletBehaviour != null)
        {
            bulletBehaviour.initialVelocity = directionToPlayer * bulletSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, targetDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * hoverHeight);
    }
}

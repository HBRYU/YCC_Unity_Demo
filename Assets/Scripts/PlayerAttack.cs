using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage = 10f;
    public float coolDown = 1f;
    private float _coolDownTimer;
    public SpriteRenderer attackGraphics;
    public Transform attackPosition;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && _coolDownTimer <= 0) // Left mouse click
        {
            PerformAttack();
            _coolDownTimer = coolDown;
        }

        if (_coolDownTimer > 0)
        {
            _coolDownTimer -= Time.deltaTime;
        }
    }

    private void PerformAttack()
    {
        // Optional: Display attack graphics
        if (attackGraphics != null)
        {
            // StartCoroutine(ShowAttackGraphics());
            _animator.SetTrigger("Attack");
        }

        // Detect enemies within attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemyLayer);

        // Apply damage to each enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();
            if (enemyStatus != null)
            {
                enemyStatus.TakeDamage(damage);
            }
        }
    }

    private IEnumerator ShowAttackGraphics()
    {
        attackGraphics.enabled = true;
        yield return new WaitForSeconds(0.1f); // Show for a short duration
        attackGraphics.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition.position, attackRange);
        }
    }

    public void Flip()
    {
        attackPosition.localPosition = new Vector3(-attackPosition.localPosition.x, attackPosition.localPosition.y);
        attackGraphics.flipX = !attackGraphics.flipX;
    }
}
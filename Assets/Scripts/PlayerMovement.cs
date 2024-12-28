using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("Sprint Parameters")]
    public float maxStamina = 100f;
    public float staminaRefillSpeed = 5f;
    public float sprintStaminaCoeff = 2f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D _rb;
    public SpriteRenderer spriteRenderer;
    private float _currentStamina;
    private bool _isGrounded;
    private bool _isSprinting;
    private PlayerAttack _playerAttack;

    [Header("Animation")]
    private Animator _animator;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentStamina = maxStamina;
        _animator = GetComponent<Animator>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        HandleMovement();
        HandleSprint();
        FlipSprite();
        CheckGround();
        HandleAnimation();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float moveSpeed = _isSprinting && _currentStamina > 0 ? speed * sprintStaminaCoeff : speed;
        
        _rb.linearVelocity = new Vector2(moveInput * moveSpeed, _rb.linearVelocity.y);

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandleSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _currentStamina > 0)
        {
            _isSprinting = true;
            _currentStamina -= Time.deltaTime * 10f;
            _currentStamina = Mathf.Clamp(_currentStamina, 0, maxStamina);
        }
        else
        {
            _isSprinting = false;
            _currentStamina += Time.deltaTime * staminaRefillSpeed;
            _currentStamina = Mathf.Clamp(_currentStamina, 0, maxStamina);
        }
    }

    private void FlipSprite()
    {
        if (_rb.linearVelocity.x > 0.1f)
        {
            if(spriteRenderer.flipX)
                _playerAttack.Flip();
            spriteRenderer.flipX = false;
        }
        else if (_rb.linearVelocity.x < -0.1f)
        {
            if(!spriteRenderer.flipX)
                _playerAttack.Flip();
            spriteRenderer.flipX = true;
        }
    }

    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void HandleAnimation()
    {
        _animator.SetBool("walking", Mathf.Abs(_rb.linearVelocity.x) > 0.25f);
        _animator.SetBool("phase", _isSprinting);
    }
}

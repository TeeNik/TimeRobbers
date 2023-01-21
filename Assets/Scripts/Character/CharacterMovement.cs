using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : CharacterMovementBase
{
    [SerializeField] private float _jumpForce = 700f;
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;
    [SerializeField] private bool _airControl = false;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private float _runSpeed = 40f;

    [SerializeField] private GroundCollider _groundCollider;

    const float _groundedRadius = .2f;
    private bool _grounded;
    const float _ceilingRadius = .2f;
    private Rigidbody2D _rigidbody2D;
    private bool _facingRight = true;
    private Vector3 _velocity = Vector3.zero;
    private Animator _animator;
    private bool _isDead;

    public override void Move(Vector2 move, bool jump)
    {
        if (!_isDead)
        {
            _animator.SetBool("IsRunning", move.x != 0);
            _animator.SetBool("IsInAir", !_grounded);

            if (_grounded || _airControl)
            {
                Vector3 targetVelocity = new Vector2(move.x * 10f, _rigidbody2D.velocity.y);
                _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);
                if (move.x > 0 && !_facingRight || move.x < 0 && _facingRight)
                {
                    Flip();
                }
            }
            if (_grounded && jump)
            {
                _grounded = false;
                _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
            }
        }
    }

    public override void AddJumpForce(float multiplier) 
    {
        _rigidbody2D.AddForce(new Vector2(0f, _jumpForce * multiplier));
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public override void Freeze()
    {
        StopMovement();
        _animator.speed = 0;
    }

    public override void PlayDieAnimation()
    {
        _isDead = true;
        StopMovement();
        _animator.SetTrigger("Death");
    }

    private void FixedUpdate()
    {
        _grounded = _groundCollider.IsTouchingGround;
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    public override Vector2 ActionToSpeed(InputController.Action action)
    {
        Vector2 speed = Vector2.zero;
        if (action.HasFlag(InputController.Action.Left))
        {
            speed.x -= _runSpeed;
        }
        if (action.HasFlag(InputController.Action.Right))
        {
            speed.x += _runSpeed;
        }
        return speed;
    }

    private void StopMovement()
    {
        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.velocity = Vector2.zero;
    }
}

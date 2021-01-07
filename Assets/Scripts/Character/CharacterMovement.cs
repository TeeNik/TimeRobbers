using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
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

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        _grounded = _groundCollider.IsTouchingGround;
        //_grounded = false;
        //
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _whatIsGround);
        //for (int i = 0; i < colliders.Length; i++)
        //{
        //    if (colliders[i].gameObject != gameObject)
        //        _grounded = true;
        //}
    }

    public void Move(float move, bool jump)
    {
        _animator.SetBool("IsRunning", move != 0);
        _animator.SetBool("IsInAir", !_grounded);

        if (_grounded || _airControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);
            if (move > 0 && !_facingRight || move < 0 && _facingRight)
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

    public void AddJumpForce(float multiplier)
    {
        _rigidbody2D.AddForce(new Vector2(0f, _jumpForce * multiplier));
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    public float ActionToSpeed(InputController.Action action)
    {
        var horizontalMove = 0f;
        if (action.HasFlag(InputController.Action.Left))
        {
            horizontalMove -= _runSpeed;
        }
        if (action.HasFlag(InputController.Action.Right))
        {
            horizontalMove += _runSpeed;
        }
        return horizontalMove;
    }
}

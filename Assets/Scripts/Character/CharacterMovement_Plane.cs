using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement_Plane : CharacterMovementBase
{
    [SerializeField] private float _runSpeed = 40f;
    [Range(0, .3f)][SerializeField] private float _movementSmoothing = .05f;

    private Rigidbody2D _rigidbody2D;
    private bool _facingRight = true;
    private Vector3 _velocity = Vector3.zero;
    private Animator _animator;
    private bool _isDead;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
    }

    public override void Move(Vector2 move, bool jump)
    {
        if (!_isDead)
        {
            _animator.SetBool("IsRunning", move.x != 0);

            Vector3 targetVelocity = new Vector2(move.x * 10f, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);
            if (move.x > 0 && !_facingRight || move.x < 0 && _facingRight)
            {
                Flip();
            }
        }
    }

    public override void AddJumpForce(float multiplier)
    {
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
        if (action.HasFlag(InputController.Action.Up))
        {
            speed.y -= _runSpeed;
        }
        if (action.HasFlag(InputController.Action.Down))
        {
            speed.y += _runSpeed;
        }
        return speed;
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

    void StopMovement()
    {
        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.velocity = Vector2.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float _timeBeforeFall = 1.5f;
    [SerializeField] private float _fallDistance = 10f;
    [SerializeField] private float _fallVelocity = 2f;

    private SpeedComponent _speedComponent;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private bool _isFalling;
    private float _time = 0f;
    private float _y;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _speedComponent = GetComponent<SpeedComponent>();
        //_animator = GetComponent<Animator>();
        _y = transform.position.y - _fallDistance;
    }

    void FixedUpdate()
    {
        if (_isFalling)
        {
            _time += Time.fixedDeltaTime * _speedComponent.TimeScale;

            if (_time >= _timeBeforeFall)
            {
                //_rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, _fallVelocity, ref _velocity, _movementSmoothing);
                //
                //transform.position = Vector3.MoveTowards(transform.position, )
            }

        }
    }

    void StartFalling()
    {
        _isFalling = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(StringTags.GroundCheck))
        {
            StartFalling();
        }
    }
}

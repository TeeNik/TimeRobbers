using UnityEngine;

[RequireComponent(typeof(SpeedComponent), typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public Vector3 Velocity;
    [Range(0, .3f)] public float MovementSmoothing = .05f;


    private SpeedComponent _speedComponent;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _velocity;

    void Awake()
    {
        _speedComponent = GetComponent<SpeedComponent>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = Velocity * _speedComponent.TimeScale;
        _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, MovementSmoothing);
    }
}

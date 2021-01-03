using UnityEngine;

public class Forcer : MonoBehaviour
{
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;

    private SpeedComponent _speedComponent;

    private GameObject _target;
    private bool _movingForward;
    private bool _movingBackward;

    private Vector3 _translateVector;

    void Awake()
    {
        _speedComponent = GetComponent<SpeedComponent>();
    }

    void FixedUpdate()
    {
        if (_movingForward)
        {
            transform.Translate(_translateVector * Time.fixedDeltaTime * 12 * _speedComponent.TimeScale);

            if (transform.position.y >= _endPos.position.y)
            {
                _movingForward = false;
                _movingBackward = true;
                _translateVector = _startPos.position - transform.position;
                if (_target != null)
                {
                    var character = _target.GetComponent<CharacterMovement>();
                    character.AddJumpForce(3);
                }
            }
        }
        else if (_movingBackward)
        {
            transform.Translate(_translateVector * Time.fixedDeltaTime * 2f * _speedComponent.TimeScale);
            if (transform.position.y <= _startPos.position.y)
            {
                _movingBackward = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == StringTags.GroundCheck)
        {
            if (!_movingForward)
            {
                _translateVector = _endPos.position - transform.position;
                _movingForward = true;
                _target = collider.transform.parent.gameObject;
                _target.transform.parent = transform;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (_target != null)
        {
            _target.transform.parent = null;
            _target = null;
        }
    }

}

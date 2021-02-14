using UnityEngine;

public class Forcer : MonoBehaviour
{
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;
    [SerializeField] private float _upTime = 0.25f;
    [SerializeField] private float _downTime = 0.5f;
    [SerializeField] private float _maxForce = 3;

    private SpeedComponent _speedComponent;

    private GameObject _target;
    private bool _movingForward;
    private bool _movingBackward;

    private Vector3 _translateVector;
    private float _force;
    private float _time = 0;

    void Awake()
    {
        _speedComponent = GetComponent<SpeedComponent>();
    }

    void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime * _speedComponent.TimeScale;
        if (_movingForward)
        {
            float step = (_endPos.position - _startPos.position).magnitude / _upTime;
            transform.position = Vector3.MoveTowards(transform.position, _endPos.position, deltaTime * step);
            _time += deltaTime * _speedComponent.TimeScale;
            _force = Mathf.Lerp(0, _maxForce, _time / _upTime);
            //Debug.Log(_time);

            if (Vector3.SqrMagnitude(transform.position - _endPos.position) < 0.0001f)
            {
                _movingForward = false;
                _movingBackward = true;
                _translateVector = _startPos.position - transform.position;
                ApplyForceToTarget();
            }

            /*transform.Translate(_translateVector * deltaTime * _upSpeed);
            Debug.Log(_force);
            if (transform.position.y >= _endPos.position.y)
            {

                if (_target != null)
                {
                    var character = _target.GetComponent<CharacterMovement>();
                    character.AddJumpForce(_force);
                }
            }*/
        }
        else if (_movingBackward)
        {
            float step = (_endPos.position - _startPos.position).magnitude / _downTime;
            transform.position = Vector3.MoveTowards(transform.position, _startPos.position, deltaTime * step);
            if (Vector3.SqrMagnitude(transform.position - _startPos.position) < 0.0001f)
            //transform.Translate(_translateVector * deltaTime * 2f);
            //if (transform.position.y <= _startPos.position.y)
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
        ApplyForceToTarget();
        //if (_target != null)
        //{
        //    _target.transform.parent = null;
        //    _target = null;
        //}
    }
    void ApplyForceToTarget()
    {
        if (_target != null && _target.gameObject.activeSelf)
        {
            _target.transform.parent = null;
            var character = _target.GetComponent<CharacterMovement>();
            //Debug.Log("Force applied: " + _force);
            character.AddJumpForce(_force);
            _target = null;
        }
        _force = 0;
        _time = 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public CharacterController controller;
    public CharacterController copyPrefab;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    private CharacterController _copy = null;
    private float _time = 0;
    private float _spawnTime = 0;

    private enum Action
    {
        None,
        Left,
        Right
    }

    private Queue<KeyValuePair<float, float>> _history = new Queue<KeyValuePair<float, float>>();
    private Dictionary<float, float> _actions = new Dictionary<float, float>();
    private float _last = -1;


    private bool _inUpdate = true;

    void HandleInput()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        print(horizontalMove);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _inUpdate = !_inUpdate;
            print("inUpdate: " + _inUpdate);

        }

        if (_last != horizontalMove)
        {
            _actions.Add(_time, horizontalMove);
            _history.Enqueue(new KeyValuePair<float, float>(_time, horizontalMove));
            _last = horizontalMove;
            //print(_time + "  " + horizontalMove);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            var pos = transform.position;
            pos.x = 0;
            _copy = Instantiate(copyPrefab, pos, transform.rotation);
            _spawnTime = _time;
        }
        _time += Time.deltaTime;
    }

    void Update()
    {
        if (_inUpdate)
        {
            HandleInput();
        }

    }

    private float _copySpeed = 0;
    void FixedUpdate()
    {
        if (!_inUpdate)
        {
            HandleInput();
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;

        if (_copy != null)
        {
            if (_history.Count > 0)
            {
                var first = _history.Peek();
                if (first.Key <= _time - _spawnTime)
                {
                    _history.Dequeue();
                    _copySpeed = first.Value;
                }
            }
            _copy.Move(_copySpeed * Time.fixedDeltaTime, false);
        }
    }
}

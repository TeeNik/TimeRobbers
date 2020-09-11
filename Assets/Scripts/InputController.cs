using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public CharacterController controller;
    public CharacterController copyPrefab;

    public float runSpeed = 40f;

    private CharacterController _copy = null;
    private float _time = 0;
    private float _spawnTime = 0;

    [Flags]
    private enum  Action
    {
        Empty = 0,
        Stay = 1,
        Left = 2,
        Right = 4,
        Jump = 8,
    }

    private Queue<KeyValuePair<float, Action>> _history = new Queue<KeyValuePair<float, Action>>();

    private struct InputState
    {
        public bool Left;
        public bool Right;
        public bool Jump;
        public bool Copy;

        public void Clear()
        {
            Left = false;
            Right = false;
            Jump = false;
            Copy = false;
        }

        public bool NeedRecord()
        {
            return Left || Right || Jump;
        }
    }

    private InputState _input = new InputState();
    private Action _action;
    private Action _last = Action.Empty;

    private float ActionToSpeed(Action action)
    {
        var horizontalMove = 0f;
        if (action.HasFlag(Action.Left))
        {
            horizontalMove -= runSpeed;
        }
        if (action.HasFlag(Action.Right))
        {
            horizontalMove += runSpeed;
        }
        return horizontalMove;
    }

    void Update()
    {

        print("A: " + Input.GetKey(KeyCode.A));

        if (!_input.Left && Input.GetKey(KeyCode.A))
        {
            _input.Left = true;
            _action |= Action.Left;
        }
        if (!_input.Right && Input.GetKey(KeyCode.D))
        {
            _input.Right = true;
            _action |= Action.Right;
        }
        if (!_input.Jump && Input.GetButtonDown("Jump"))
        {
            _input.Jump = true;
            _action |= Action.Jump;
        }
        if (!_input.Copy && Input.GetKeyDown(KeyCode.R))
        {
            _input.Copy = true;
        }
    }

    private float _copySpeed = 0;

    void FixedUpdate()
    {
        if (_last != _action)
        {
            _history.Enqueue(new KeyValuePair<float, Action>(_time, _action));
            _last = _action;
        }

        if (_input.Copy)
        {
            var pos = transform.position;
            pos.x = 0;
            _copy = Instantiate(copyPrefab, pos, transform.rotation);
            _spawnTime = _time;
        }
        controller.Move(ActionToSpeed(_action) * Time.fixedDeltaTime, _input.Jump);

        _input.Clear();
        _action = Action.Empty;

        if (_copy != null)
        {
            var copySpeed = 0f;
            var jump = false;
            if (_history.Count > 0)
            {
                var first = _history.Peek();
                if (first.Key <= _time - _spawnTime)
                {
                    _history.Dequeue();
                    _copySpeed = ActionToSpeed(first.Value);
                    jump = first.Value.HasFlag(Action.Jump);
                }
            }
            _copy.Move(_copySpeed * Time.fixedDeltaTime, jump);
        }

        _time += Time.fixedDeltaTime;
    }
}

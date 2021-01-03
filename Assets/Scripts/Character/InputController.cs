using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InputController : MonoBehaviour
{
    public CharacterMovement Movement;
    public BaseCharacter BaseCharacter;

    private float _time = 0;
    private bool _isRecording = true;

    [Flags]
    public enum  Action
    {
        Empty = 0,
        Left = 1,
        Right = 2,
        Jump = 4,
        Action = 8,
    }

    private List<KeyValuePair<float, Action>> _history = new List<KeyValuePair<float, Action>>();

    private Action _action;
    private Action _last = Action.Empty;

    void Update()
    {
        if (_isRecording)
        {
            if (!_action.HasFlag(Action.Left) && Input.GetKey(KeyCode.A))
            {
                _action |= Action.Left;
            }
            if (!_action.HasFlag(Action.Right) && Input.GetKey(KeyCode.D))
            {
                _action |= Action.Right;
            }
            if (!_action.HasFlag(Action.Jump) && Input.GetKeyDown(KeyCode.W))
            {
                _action |= Action.Jump;
            }
            if (!_action.HasFlag(Action.Action) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                _action |= Action.Action;
            }
        }
    }

    void FixedUpdate()
    {
        if (_last != _action)
        {
            _history.Add(new KeyValuePair<float, Action>(_time, _action));
            _last = _action;
        }

        if (_action.HasFlag(Action.Action))
        {
            StopRecording();
            BaseCharacter.Action();
        }
        else
        {
            Movement.Move(Movement.ActionToSpeed(_action) * Time.fixedDeltaTime, _action.HasFlag(Action.Jump));
        }

        _action = Action.Empty;
        _time += Time.fixedDeltaTime;
    }

    public void StopRecording()
    {
        Assert.IsTrue(_isRecording);

        _isRecording = false;
        PlanningStage.Instance.Save(_history);
    }
}

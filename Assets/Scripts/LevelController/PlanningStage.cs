using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningStage : MonoBehaviour
{

    [SerializeField] private PlanningView _view;

    [SerializeField] private CharacterController _charPrefab;
    [SerializeField] private Transform _spawnPoint;

    public struct TurnInfo
    {
        public float Speed;
        public CharacterController Character;
        public Queue<KeyValuePair<float, InputController.Action>> History;
    }

    private List<TurnInfo> _turns = new List<TurnInfo>();
    private bool _isPlaying = false;

    private float _time = 0;

    public static PlanningStage Instance { get; private set; }

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (!_isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Cleared");
                _turns.Clear();
                _view.SetNumberOfRecords(0);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _isPlaying = true;
                for (var i = 0; i < _turns.Count; ++i)
                {
                    var turn = _turns[i];
                    turn.Character = Instantiate(_charPrefab, _spawnPoint) as CharacterController;
                }
            }
        }
    }

    private float runSpeed = 40;
    private float ActionToSpeed(InputController.Action action)
    {
        var horizontalMove = 0f;
        if (action.HasFlag(InputController.Action.Left))
        {
            horizontalMove -= runSpeed;
        }
        if (action.HasFlag(InputController.Action.Right))
        {
            horizontalMove += runSpeed;
        }
        return horizontalMove;
    }

    void FixedUpdate()
    {
        if (_isPlaying)
        {
            for (var i = 0; i < _turns.Count; ++i)
            {
                var turn = _turns[i];
                var jump = false;
                if (turn.History.Count > 0)
                {
                    var first = turn.History.Peek();
                    if (first.Key <= _time /* - _spawnTime*/)
                    {
                        turn.History.Dequeue();
                        turn.Speed = ActionToSpeed(first.Value);
                        jump = first.Value.HasFlag(InputController.Action.Jump);
                    }
                }
                turn.Character.Move(turn.Speed * Time.fixedDeltaTime, jump);
            }

            _time += Time.fixedDeltaTime;
        }
    }


    public void Save(Queue<KeyValuePair<float, InputController.Action>> history)
    {
        var item = new TurnInfo();
        item.History = history;
        _turns.Add(item);
    }

 }

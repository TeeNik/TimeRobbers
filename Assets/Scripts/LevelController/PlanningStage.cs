using System.Collections.Generic;
using UnityEngine;

public class PlanningStage : MonoBehaviour
{
    private class TurnInfo
    {
        public int CharacterType;
        public List<KeyValuePair<float, InputController.Action>> History;
    }

    private class ReplayInfo
    {
        public float Speed;
        public BaseCharacter Character;
        public int TurnIndex;
    }


    [SerializeField] private PlanningView _view;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private BaseCharacter[] _characters;

    private int _characterType = 0;

    private readonly List<TurnInfo> _turns = new List<TurnInfo>();
    private readonly List<ReplayInfo> _replays = new List<ReplayInfo>();

    private List<ReplayUpdateComponent> _replayUpdatesComponents = new List<ReplayUpdateComponent>();

    private bool _isPlaying = false;
    private float _time = 0;

    public static PlanningStage Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _view.SetVisibility(true);
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
               Instantiate(_characters[_characterType], _spawnPoint.position, Quaternion.identity);
               PrepareReplay();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                PrepareReplay();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                _characterType = _characterType == 0 ? _characters.Length - 1 : _characterType - 1;
                Debug.Log(_characters[_characterType].name);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _characterType = _characterType == _characters.Length - 1 ? 0 : _characterType + 1;
                Debug.Log(_characters[_characterType].name);
            }
        }
    }

    void FixedUpdate()
    {
        if (_isPlaying)
        {
            for (var i = 0; i < _turns.Count; ++i)
            {
                var turn = _turns[i];
                var replay = _replays[i];

                if (replay.Character.IsDead)
                {
                    continue;
                }

                var jump = false;
                if (replay.TurnIndex < turn.History.Count)
                {
                    var first = turn.History[replay.TurnIndex];
                    if (first.Key <= _time)
                    {
                        ++replay.TurnIndex;
                        replay.Speed = replay.Character.CharacterMovement.ActionToSpeed(first.Value);
                        jump = first.Value.HasFlag(InputController.Action.Jump);

                        if (first.Value == InputController.Action.Ability)
                        {
                            replay.Character.UseAbility();
                        }
                    }
                }
                replay.Character.CharacterMovement.Move(replay.Speed * Time.fixedDeltaTime, jump);
            }

            _time += Time.fixedDeltaTime;
        }
    }

    public void Save(List<KeyValuePair<float, InputController.Action>> history)
    {
        var item = new TurnInfo();
        item.History = history;
        item.CharacterType = _characterType;
        _turns.Add(item);
        _view.SetNumberOfRecords(_turns.Count);
        _characterType = 0;
        _view.SetVisibility(true);
        _isPlaying = false;
    }

    private void PrepareReplay()
    {
        _view.SetVisibility(false);
        foreach (var replay in _replays)
        {
            if (replay.Character != null)
            {
                Destroy(replay.Character.gameObject);
            }
        }
        _replays.Clear();

        foreach (var turn in _turns)
        {
            ReplayInfo replay = new ReplayInfo();
            replay.Character = Instantiate(_characters[turn.CharacterType], _spawnPoint.position, Quaternion.identity);
            replay.Character.InputController.enabled = false;
            _replays.Add(replay);
        }

        foreach (var component in _replayUpdatesComponents)
        {
            component.OnBeforeReplay();
        }
        foreach (var component in _toRemove)
        {
            _replayUpdatesComponents.Remove(component);
        }
        _toRemove.Clear();

        _time = 0;
        _isPlaying = true;
    }

    private List<ReplayUpdateComponent> _toRemove = new List<ReplayUpdateComponent>();

    public void RegisterReplayUpdateComponent(ReplayUpdateComponent component)
    {
        _replayUpdatesComponents.Add(component);
    }

    //TODO refactor
    public void UnregisterReplayUpdateComponent(ReplayUpdateComponent component)
    {
        _toRemove.Add(component);
    }
 }
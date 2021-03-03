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
    [SerializeField] private BaseCharacter[] _characters;

    private int _characterType = 0;

    private readonly List<TurnInfo> _turns = new List<TurnInfo>();
    private readonly List<ReplayInfo> _replays = new List<ReplayInfo>();

    private List<ReplayUpdateComponent> _replayUpdatesComponents = new List<ReplayUpdateComponent>();

    private bool _isPlaying = false;
    private float _time = 0;

    private bool _isPlanningEnabled = false;
    private Level _level;

    public void Init()
    {
        _view.Init(OnCharacterSelected, OnHistoryItemDeleted);
    }

    public void InitLevel(Level level)
    {
        _level = level;
        _isPlanningEnabled = level.AllowedCharacters.Length > 1;
        _view.InitLevel(level);
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

                        if (first.Value.HasFlag(InputController.Action.Ability))
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

    void OnCharacterSelected(int selectedCharacter)
    {
        _characterType = selectedCharacter;
        Instantiate(_characters[_characterType], _level.SpawnPoint, Quaternion.identity);
        PrepareReplay();
    }

    void OnHistoryItemDeleted(int index)
    {
        _turns.RemoveAt(index);
    }

    public void Save(List<KeyValuePair<float, InputController.Action>> history)
    {
        if (_isPlanningEnabled)
        {
            var item = new TurnInfo();
            item.History = history;
            item.CharacterType = _characterType;
            _turns.Add(item);
            _view.CreateHistoryItem(_characterType);
            _characterType = 0;
            SetIsPlaying(false);
            _view.SetVisibility(true);
        }
        else
        {
            OnCharacterSelected(0);
        }
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
            replay.Character = Instantiate(_characters[turn.CharacterType], _level.SpawnPoint, Quaternion.identity);
            replay.Character.GetComponent<InputController>().enabled = false;
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
        SetIsPlaying(true);
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

    private void SetIsPlaying(bool isPlaying)
    {
        _isPlaying = isPlaying;
        GameInstance.Instance.IsPlaying = isPlaying;
    }
 }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningStage : MonoBehaviour
{

    [SerializeField] private PlanningView _view;

    public struct TurnInfo
    {
        public float Speed;
        public CharacterController Character;
        public Queue<KeyValuePair<float, InputController.Action>> History;
    }

    private List<TurnInfo> turns = new List<TurnInfo>();
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
                turns.Clear();
                _view.SetNumberOfRecords(0);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _isPlaying = true;
                foreach (var turn in turns)
                {
                    turn.Character =
                }
            }
        }
        else
        {
            
        }
    }

    void FixedUpdate()
    {
        if (_isPlaying)
        {

        }
    }


    public void Save(Queue<KeyValuePair<float, InputController.Action>> history)
    {
        var item = new TurnInfo();
        item.History = history;
        turns.Add(item);
    }

 }

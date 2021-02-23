using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    private static GameInstance _instance;

    public PlanningStage PlanningStage;

    [SerializeField] private Level[] _levels;
    private int _currentLevelIndex;
    private Level _loadedLevel;

    public static GameInstance Instance => _instance;

    [HideInInspector]
    public bool IsPlaying;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        PlanningStage.Init();
        LoadCurrentLevel();
    }

    public void LoadCurrentLevel()
    {
        _loadedLevel = Instantiate(_levels[_currentLevelIndex]);
        PlanningStage.InitLevel(_loadedLevel);
    }

    public void FinishLevel()
    {
        Debug.Log("level finished");
    }
}

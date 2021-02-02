using UnityEngine;

public class GameInstance
{
    private static GameInstance _instance;
    public static GameInstance Instance
    {
        get => _instance ?? (_instance = new GameInstance());
        private set => _instance = value;
    }

    public bool IsPlaying;

    public void FinishLevel()
    {
        Debug.Log("level finished");
    }
}

using System;
using UnityEngine;

public class ReplayUpdateComponent : MonoBehaviour
{
    private Action _beforeReplayAction = null;

    void Start()
    {
        GameInstance.Instance.PlanningStage.RegisterReplayUpdateComponent(this);
    }

    public void Unregister()
    {
        GameInstance.Instance.PlanningStage.UnregisterReplayUpdateComponent(this);
    }

    public void SetBeforeReplayAction(Action beforeReplayAction)
    {
        _beforeReplayAction = beforeReplayAction;
    }

    public void OnBeforeReplay()
    {
        _beforeReplayAction?.Invoke();
    }
}

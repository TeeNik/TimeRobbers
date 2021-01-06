using System;
using UnityEngine;

public class ReplayUpdateComponent : MonoBehaviour
{
    private Action _beforeReplayAction = null;

    void Start()
    {
        PlanningStage.Instance.RegisterReplayUpdateComponent(this);
    }

    public void Unregister()
    {
        PlanningStage.Instance.UnregisterReplayUpdateComponent(this);
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

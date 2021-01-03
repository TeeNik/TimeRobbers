using System;
using UnityEngine;

public class ReplayUpdateComponent : MonoBehaviour
{
    private Action _beforeReplayAction = null;

    public void SetBeforeReplayAction(Action beforeReplayAction)
    {
        _beforeReplayAction = beforeReplayAction;
    }

    public void OnBeforeReplay()
    {
        _beforeReplayAction?.Invoke();
    }
}

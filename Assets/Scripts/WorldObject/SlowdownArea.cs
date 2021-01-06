using UnityEngine;

public class SlowdownArea : MonoBehaviour
{
    public float TimeScaleDecrease = 0.5f;

    private ReplayUpdateComponent _replayUpdateComponent;
    void Awake()
    {
        _replayUpdateComponent = GetComponent<ReplayUpdateComponent>();
        _replayUpdateComponent.SetBeforeReplayAction(() =>
        {
            _replayUpdateComponent.Unregister();
            Destroy(gameObject);
        });
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(typeof(SpeedComponent), out var component))
        {
            SpeedComponent speedComponent = (SpeedComponent) component;
            speedComponent.TimeScale -= TimeScaleDecrease;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(typeof(SpeedComponent), out var component))
        {
            SpeedComponent speedComponent = (SpeedComponent)component;
            speedComponent.TimeScale += TimeScaleDecrease;
        }
    }
}

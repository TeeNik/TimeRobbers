using UnityEngine;

public class SlowdownArea : MonoBehaviour
{
    public float TimeScaleDecrease = 0.5f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        Component component;
        if (collider.gameObject.TryGetComponent(typeof(SpeedComponent), out component))
        {
            SpeedComponent speedComponent = (SpeedComponent) component;
            speedComponent.TimeScale -= TimeScaleDecrease;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Component component;
        if (collider.gameObject.TryGetComponent(typeof(SpeedComponent), out component))
        {
            SpeedComponent speedComponent = (SpeedComponent)component;
            speedComponent.TimeScale += TimeScaleDecrease;
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(ReplayUpdateComponent))]
public class FlippingSpikes : MonoBehaviour
{
    private ReplayUpdateComponent _replayUpdateComponent;
    private Vector3 _initialRotation;

    void Awake()
    {
        _replayUpdateComponent = GetComponent<ReplayUpdateComponent>();
    }

    void Start()
    {
        _initialRotation = transform.eulerAngles;
        _replayUpdateComponent.SetBeforeReplayAction(OnBeforeReplay);
        PlanningStage.Instance.RegisterReplayUpdateComponent(_replayUpdateComponent);
    }

    void OnBeforeReplay()
    {
        transform.eulerAngles = _initialRotation;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == StringTags.GroundCheck)
        {
            transform.Rotate(Vector3.right, 180);
        }
    }
}

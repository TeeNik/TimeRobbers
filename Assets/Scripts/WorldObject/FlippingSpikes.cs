using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(ReplayUpdateComponent))]
public class FlippingSpikes : MonoBehaviour
{
    [SerializeField] private float _rotationDuration = 0.25f;

    private ReplayUpdateComponent _replayUpdateComponent;
    private Vector3 _initialRotation;

    void Awake()
    {
        _replayUpdateComponent = GetComponent<ReplayUpdateComponent>();
        _initialRotation = transform.eulerAngles;
        _replayUpdateComponent.SetBeforeReplayAction(ResetToInitial);
    }

    void ResetToInitial()
    {
        transform.eulerAngles = _initialRotation;
    }

    public void RotateAround()
    {
        transform.DORotate(transform.eulerAngles + new Vector3(180, 0, 0), _rotationDuration);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == StringTags.GroundCheck && transform.eulerAngles == _initialRotation)
        {
            RotateAround();
        }
    }
}

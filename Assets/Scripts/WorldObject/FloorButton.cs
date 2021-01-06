using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class FloorButton : MonoBehaviour
{
    [SerializeField] private List<UnityEvent> _events;

    private ReplayUpdateComponent _replayUpdateComponent;
    private Vector3 _originalPosition;

    void Awake()
    {
        _originalPosition = transform.position;
        _replayUpdateComponent = GetComponent<ReplayUpdateComponent>();
        _replayUpdateComponent.SetBeforeReplayAction(ReturnToInitial);
    }

    void ReturnToInitial()
    {
        transform.position = _originalPosition;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(StringTags.GroundCheck) && transform.position == _originalPosition)
        {
            transform.DOMoveY(transform.position.y - .2f, 0.25f);
            foreach (var unityEvent in _events)
            {
                unityEvent.Invoke();
            }
        }
    }

}

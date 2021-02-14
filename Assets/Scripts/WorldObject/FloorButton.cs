using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class FloorButton : MonoBehaviour
{
    [SerializeField] private List<UnityEvent> _events;
    [SerializeField] private List<SpriteRenderer> _wires;

    private ReplayUpdateComponent _replayUpdateComponent;
    private Vector3 _originalPosition;
    private Color _originalWireColor;

    void Awake()
    {
        _originalPosition = transform.position;
        _originalWireColor = _wires.Count > 0 ? _wires[0].color : Color.black;
        _replayUpdateComponent = GetComponent<ReplayUpdateComponent>();
        _replayUpdateComponent.SetBeforeReplayAction(ResetToInitial);
    }

    void ResetToInitial()
    {
        transform.position = _originalPosition;
        foreach (var wire in _wires)
        {
            wire.color = _originalWireColor;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(StringTags.GroundCheck) && transform.position == _originalPosition)
        {
            transform.DOMoveY(transform.position.y - .2f, 0.25f);
            foreach (var unityEvent in _events)
            {
                unityEvent.Invoke();
                BlinkWire();
            }
        }
    }

    void BlinkWire()
    {
        foreach (var wire in _wires)
        {
            wire.DOFade(.75f, .25f).OnComplete(() =>
            {
                wire.DOFade(.25f, .25f);
            });
        }
    }
}

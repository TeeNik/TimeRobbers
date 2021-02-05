using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _enableDuration;
    [SerializeField] private float _disableDuration;
    [SerializeField] private Vector3 _enableScale;
    [SerializeField] private Vector3 _disableScale;

    [SerializeField] private Color _enableColor;
    [SerializeField] private Color _disableColor;

    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spriteRenderer;
    private ReplayUpdateComponent _replayComponent;

    private float _time = 0;
    private bool _isAble = true;

    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _replayComponent = GetComponent<ReplayUpdateComponent>();
        _replayComponent.SetBeforeReplayAction(ResetToInitial);
        _time = _isAble ? _enableDuration : _disableDuration;
    }

    void FixedUpdate()
    {
        if (GameInstance.Instance.IsPlaying)
        {
            _time -= Time.fixedDeltaTime;

            if (_time <= 0)
            {
                _isAble = !_isAble;
                _time = _isAble ? _enableDuration : _disableDuration;
                _boxCollider.enabled = _isAble;

                _spriteRenderer.DOColor(_isAble ? _enableColor : _disableColor, 0.1f);
                transform.DOScale(_isAble ? _enableScale : _disableScale, .1f);
            }
        }
    }

    void ResetToInitial()
    {
        transform.localScale = _enableScale;
        _spriteRenderer.color = _enableColor;
        _isAble = true;
        _time = _isAble ? _enableDuration : _disableDuration;
    }
}
